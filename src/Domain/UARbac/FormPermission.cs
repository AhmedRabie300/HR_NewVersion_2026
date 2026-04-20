using Domain.Common;
using Domain.UARbac;
namespace Domain.UARbac
{
    public class FormPermission : LegacyEntity
    {
        public int FormId { get; private set; }
        public int? GroupId { get; private set; }
        public int? UserId { get; private set; }
        public bool? AllowView { get; private set; }
        public bool? AllowAdd { get; private set; }
        public bool? AllowEdit { get; private set; }
        public bool? AllowDelete { get; private set; }
        public bool? AllowPrint { get; private set; }
        public int? RegUserId { get; private set; }
        public int? regComputerId { get; private set; }
        public DateTime? CancelDate { get; private set; }

        // Navigation properties
        public Form? Form { get; private set; }
        public Group? Group { get; private set; }
        public Users? User { get; private set; }

        private FormPermission() { } // For EF Core

        public FormPermission(
            int formId,
            int? groupId,
            int? userId,
            bool? allowView,
            bool? allowAdd,
            bool? allowEdit,
            bool? allowDelete,
            bool? allowPrint,
            int? regUserId,
            int? regComputerId)
        {
            FormId = formId;
            GroupId = groupId;
            UserId = userId;
            AllowView = allowView;
            AllowAdd = allowAdd;
            AllowEdit = allowEdit;
            AllowDelete = allowDelete;
            AllowPrint = allowPrint;
            RegUserId = regUserId;
            regComputerId = regComputerId;
            RegDate = DateTime.Now;
        }

        public void UpdatePermissions(
            bool? allowView,
            bool? allowAdd,
            bool? allowEdit,
            bool? allowDelete,
            bool? allowPrint)
        {
            if (allowView.HasValue) AllowView = allowView;
            if (allowAdd.HasValue) AllowAdd = allowAdd;
            if (allowEdit.HasValue) AllowEdit = allowEdit;
            if (allowDelete.HasValue) AllowDelete = allowDelete;
            if (allowPrint.HasValue) AllowPrint = allowPrint;
        }

        public void Cancel(int? regUserId = null)
        {
            CancelDate = DateTime.Now;
            if (regUserId.HasValue) RegUserId = regUserId;
        }

        public bool HasAnyPermission()
        {
            return AllowView == true ||
                   AllowAdd == true ||
                   AllowEdit == true ||
                   AllowDelete == true ||
                   AllowPrint == true;
        }
    }
}