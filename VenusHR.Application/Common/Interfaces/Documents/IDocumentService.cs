using System;
using System.IO;

namespace VenusHR.Application.Common.Interfaces.Documents
{
    public interface IDocumentsService
    {
         object SaveAttachment(int documentId, int objectId, long recordId, Stream fileStream,
                             string fileName, string engName, string arbName,
                             DateTime? issueDate = null, int? issuedCityId = null,
                             DateTime? expiryDate = null, string? documentNumber = null,
                             string? referenceNumber = null, DateTime? lastRenewalDate = null,
                             string? folderName = null);

         object GetAttachments(int objectId, long recordId);

         object DeleteAttachment(int attachmentId);

         object GetAttachmentInfo(int attachmentId);

         object AddDocumentDetail(int documentId, int objectId, int recordId, string documentNumber,
                                 DateTime? issueDate, int? issuedCityId, DateTime? expiryDate,
                                 string? referenceNumber = null, DateTime? lastRenewalDate = null);

         object GetDocumentDetails(int objectId, int recordId);

         object GetDocumentDetail(int documentDetailId);

         object UpdateDocumentDetail(int documentDetailId, string? documentNumber = null,
                                    DateTime? issueDate = null, int? issuedCityId = null,
                                    DateTime? expiryDate = null, DateTime? lastRenewalDate = null,
                                    string? referenceNumber = null);

         object DeleteDocumentDetail(int documentDetailId);

         object GetDocumentTypes(bool? isForCompany = null, int? documentTypesGroupId = null);
    }
}