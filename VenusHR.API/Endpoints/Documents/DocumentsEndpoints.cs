// DocumentsEndpoints.cs
using Microsoft.AspNetCore.Mvc;
using VenusHR.API.Controllers;
using VenusHR.Application.Common.Interfaces.Documents;
using WorkFlow_EF;

namespace VenusHR.API.Endpoints
{
    public static class DocumentsEndpoints
    {
        public static void MapDocumentsEndpoints(this WebApplication app)
        {
            // 🔹 1. رفع المرفقات
            app.MapPost("/api/documents/upload-attachment", UploadAttachment);

            // 🔹 2. إضافة مستند
            app.MapPost("/api/documents/add-document", AddDocument);

            // 🔹 3. الحصول على المرفقات
            app.MapGet("/api/documents/attachments", GetAttachments);

            // 🔹 4. تحميل المرفق
            app.MapGet("/api/documents/download/{attachmentId}", DownloadAttachment);

            // 🔹 5. حذف المرفق
            app.MapDelete("/api/documents/delete-attachment/{attachmentId}", DeleteAttachment);

            // 🔹 6. تفاصيل المستندات
            app.MapGet("/api/documents/document-details", GetDocumentDetails);

            // 🔹 7. أنواع المستندات
            //app.MapGet("/api/documents/document-types", GetDocumentTypes);

            // 🔹 8. تفاصيل مستند محدد
            app.MapGet("/api/documents/document-detail/{documentDetailId}", GetDocumentDetail);

            // 🔹 9. تحديث المستند
            app.MapPut("/api/documents/update-document/{documentDetailId}", UpdateDocumentDetail);

            // 🔹 10. حذف المستند
            app.MapDelete("/api/documents/delete-document/{documentDetailId}", DeleteDocumentDetail);

            // 🔹 11. معلومات المرفق
            app.MapGet("/api/documents/attachment-info/{attachmentId}", GetAttachmentInfo);
        }

        // =========== Implementation Methods ===========

        // 🔹 1. رفع المرفقات
        private static async Task<IResult> UploadAttachment(
            [FromForm] UploadAttachmentRequest request,
            [FromServices] IDocumentsService documentsService,
            [FromQuery] int Lang = 0)
        {
            try
            {
                if (request == null || request.File == null || request.File.Length == 0)
                {
                    return Results.BadRequest(new
                    {
                        Status = false,
                        Message = (Lang == 1) ? "الملف مطلوب" : "File is required"
                    });
                }

                if (request.DocumentId <= 0 || request.ObjectId <= 0 || request.RecordId <= 0)
                {
                    return Results.BadRequest(new
                    {
                        Status = false,
                        Message = (Lang == 1) ? "معرّفات المستند غير صحيحة" : "Invalid document identifiers"
                    });
                }

                using (var stream = request.File.OpenReadStream())
                {
                    var result = documentsService.SaveAttachment(
                        request.DocumentId,
                        request.ObjectId,
                        request.RecordId,
                        stream,
                        request.File.FileName,
                        request.EngName ?? request.File.FileName,
                        request.ArbName ?? request.File.FileName,
                        request.IssueDate,
                        request.IssuedCityId,
                        request.ExpiryDate,
                        request.DocumentNumber,
                        request.ReferenceNumber,
                        request.LastRenewalDate,
                        request.FolderName
                    );

                    return Results.Ok(result);
                }
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        // 🔹 2. إضافة مستند
        private static IResult AddDocument(
            [FromBody] AddDocumentRequest request,
            [FromServices] IDocumentsService documentsService,
            [FromQuery] int Lang = 0)
        {
            try
            {
                if (request == null)
                {
                    return Results.BadRequest(new
                    {
                        Status = false,
                        Message = (Lang == 1) ? "بيانات الطلب غير صحيحة" : "Invalid request data"
                    });
                }

                if (request.DocumentId <= 0 || request.ObjectId <= 0 || request.RecordId <= 0)
                {
                    return Results.BadRequest(new
                    {
                        Status = false,
                        Message = (Lang == 1) ? "معرّفات المستند غير صحيحة" : "Invalid document identifiers"
                    });
                }

                if (string.IsNullOrEmpty(request.DocumentNumber))
                {
                    return Results.BadRequest(new
                    {
                        Status = false,
                        Message = (Lang == 1) ? "رقم المستند مطلوب" : "Document number is required"
                    });
                }

                var result = documentsService.AddDocumentDetail(
                    request.DocumentId,
                    request.ObjectId,
                    request.RecordId,
                    request.DocumentNumber,
                    request.IssueDate,
                    request.IssuedCityId,
                    request.ExpiryDate,
                    request.ReferenceNumber,
                    request.LastRenewalDate
                );

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        // 🔹 3. الحصول على المرفقات
        private static IResult GetAttachments(
            [FromQuery] int ObjectId,
            [FromQuery] long RecordId,
            [FromServices] IDocumentsService documentsService,
            [FromQuery] int Lang = 0)
        {
            try
            {
                if (ObjectId <= 0 || RecordId <= 0)
                {
                    return Results.BadRequest(new
                    {
                        Status = false,
                        Message = (Lang == 1) ? "معرّفات السجل غير صحيحة" : "Invalid record identifiers"
                    });
                }

                var result = documentsService.GetAttachments(ObjectId, RecordId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        // 🔹 4. تحميل المرفق
        private static IResult DownloadAttachment(
            int attachmentId,
            [FromServices] IDocumentsService documentsService,
            [FromQuery] int Lang = 0)
        {
            try
            {
                if (attachmentId <= 0)
                {
                    return Results.BadRequest(new
                    {
                        Status = false,
                        Message = (Lang == 1) ? "معرّف المرفق غير صحيح" : "Invalid attachment ID"
                    });
                }

                var result = documentsService.GetAttachmentInfo(attachmentId);

                if (result is GeneralOutputClass<object> output && output.ErrorCode == 1)
                {
                    var resultObject = output.ResultObject;
                    if (resultObject != null)
                    {
                        var type = resultObject.GetType();
                        var fullPathProperty = type.GetProperty("FullPath");
                        var fileNameProperty = type.GetProperty("FileName");
                        var contentTypeProperty = type.GetProperty("ContentType");

                        if (fullPathProperty != null && fileNameProperty != null)
                        {
                            var filePath = fullPathProperty.GetValue(resultObject)?.ToString();
                            var fileName = fileNameProperty.GetValue(resultObject)?.ToString();
                            var contentType = contentTypeProperty?.GetValue(resultObject)?.ToString() ?? "application/octet-stream";

                            if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                            {
                                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                                return Results.File(fileBytes, contentType, fileName);
                            }
                        }
                    }
                }

                return Results.NotFound(new
                {
                    Status = false,
                    Message = (Lang == 1) ? "الملف غير موجود" : "File not found"
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        // 🔹 5. حذف المرفق
        private static IResult DeleteAttachment(
            int attachmentId,
            [FromServices] IDocumentsService documentsService,
            [FromQuery] int Lang = 0)
        {
            try
            {
                if (attachmentId <= 0)
                {
                    return Results.BadRequest(new
                    {
                        Status = false,
                        Message = (Lang == 1) ? "معرّف المرفق غير صحيح" : "Invalid attachment ID"
                    });
                }

                var result = documentsService.DeleteAttachment(attachmentId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        // 🔹 6. تفاصيل المستندات
        private static IResult GetDocumentDetails(
            [FromQuery] int ObjectId,
            [FromQuery] int RecordId,
            [FromServices] IDocumentsService documentsService,
            [FromQuery] int Lang = 0)
        {
            try
            {
                if (ObjectId <= 0 || RecordId <= 0)
                {
                    return Results.BadRequest(new
                    {
                        Status = false,
                        Message = (Lang == 1) ? "معرّفات السجل غير صحيحة" : "Invalid record identifiers"
                    });
                }

                var result = documentsService.GetDocumentDetails(ObjectId, RecordId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        // 🔹 7. أنواع المستندات
         private static IResult GetDocumentTypes(
            [FromServices] IDocumentsService documentsService,
            [FromQuery] bool? isForCompany = null,
            [FromQuery] int? documentTypesGroupId = null,
            [FromQuery] int Lang = 0)
        {
            try
            {
                // 🔥 تحويل صريح للنوع
                bool? isForCompanyValue = isForCompany.HasValue ? isForCompany.Value : (bool?)null;
                int? documentTypesGroupIdValue = documentTypesGroupId.HasValue ? documentTypesGroupId.Value : (int?)null;

                var result = documentsService.GetDocumentTypes(isForCompanyValue, documentTypesGroupIdValue);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }
        // 🔹 8. تفاصيل مستند محدد
        private static IResult GetDocumentDetail(
            int documentDetailId,
            [FromServices] IDocumentsService documentsService,
            [FromQuery] int Lang = 0)
        {
            try
            {
                if (documentDetailId <= 0)
                {
                    return Results.BadRequest(new
                    {
                        Status = false,
                        Message = (Lang == 1) ? "معرّف المستند غير صحيح" : "Invalid document ID"
                    });
                }

                var result = documentsService.GetDocumentDetail(documentDetailId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        // 🔹 9. تحديث المستند
        private static IResult UpdateDocumentDetail(
            int documentDetailId,
            [FromBody] UpdateDocumentRequest request,
            [FromServices] IDocumentsService documentsService,
            [FromQuery] int Lang = 0)
        {
            try
            {
                if (documentDetailId <= 0)
                {
                    return Results.BadRequest(new
                    {
                        Status = false,
                        Message = (Lang == 1) ? "معرّف المستند غير صحيح" : "Invalid document ID"
                    });
                }

                var result = documentsService.UpdateDocumentDetail(
                    documentDetailId,
                    request.DocumentNumber,
                    request.IssueDate,
                    request.IssuedCityId,
                    request.ExpiryDate,
                    request.LastRenewalDate,
                    request.ReferenceNumber
                );

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        // 🔹 10. حذف المستند
        private static IResult DeleteDocumentDetail(
            int documentDetailId,
            [FromServices] IDocumentsService documentsService,
            [FromQuery] int Lang = 0)
        {
            try
            {
                if (documentDetailId <= 0)
                {
                    return Results.BadRequest(new
                    {
                        Status = false,
                        Message = (Lang == 1) ? "معرّف المستند غير صحيح" : "Invalid document ID"
                    });
                }

                var result = documentsService.DeleteDocumentDetail(documentDetailId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        // 🔹 11. معلومات المرفق
        private static IResult GetAttachmentInfo(
            int attachmentId,
            [FromServices] IDocumentsService documentsService,
            [FromQuery] int Lang = 0)
        {
            try
            {
                if (attachmentId <= 0)
                {
                    return Results.BadRequest(new
                    {
                        Status = false,
                        Message = (Lang == 1) ? "معرّف المرفق غير صحيح" : "Invalid attachment ID"
                    });
                }

                var result = documentsService.GetAttachmentInfo(attachmentId);
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }
    }
}

 