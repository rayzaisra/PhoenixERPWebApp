using System;
using System.Collections.Generic;
using System.Text;

namespace Phoenix.Data.Models
{
    public static class NotificationTypeCode
    {
        #region Finance
        public const string PRNotification = "fn.pr.need-approval";
        public const string RFQNotification = "fn.rfq.need-approval";
        public const string PONotification = "fn.po.need-approval";
        public const string CashAdvanceApprovalNotification = "fn.ca.need-approval";
        public const string CashAdvanceSettlementApprovalNotification = "fn.cas.need-approval";
        public const string JournalAdjustmentNotification = "fn.ja.need-approval";
        public const string SalaryAllocationNotification = "fn.sa.need-approval";
        public const string GoodRequest = "fn.goodrequest.need-approval";
        public const string InventoryGoodReturn = "fn.goodreturn.need-approval";
        public const string FixedAssetMovement = "fn.fixedassetmovement.need-approval";
        public const string FixedAssetOutMovement = "fn.fixedassetoutmovement.need-approval";
        ////New
        public const string PettyCashNotification = "fn.pcs.need-approval";
        public const string RevaluationNotification = "fn.revaluation.need-approval";
        public const string SalaryPaymentRequestNotification = "fn.salarypaymentrequest.need-approval";


        public const string PaymentRequestNotification = "fn.payreq.need-approval";
        ////public const string BankPaymentApprovalNotification = "fn.bp.need-approval";
        public const string BankReceivedNotification = "fn.br.need-approval";
        public const string InvoiceClientMultiple = "fn.ar-invoiceclientmultiple.need-approval";
        public const string InvoiceClient = "fn.ar-invoiceclient.need-approval";
        #endregion

        #region PM
        public const string PCANotification = "pm.pca.need-approval";
        public const string PCENotification = "pm.pce.need-approval";
        public const string TimesheetNotification = "pm.timesheet.need-approval";
        public const string DAMNotification = "pm.digitalassetmanagement.need-approval";
        public const string MyDamNotification = "pm.mydam.need-approval";
        #endregion
        #region HRIS
        public const string MPPNotification = "hris.mpp.need-approval";
        public const string PdrPerformance = "hris.pdrperformance-approval";
        public const string PromotionRequestApproval = "hris.promotion.request-approval";
        public const string RotationRequestApproval = "hris.rotation.request-approval";
        public const string MediaPlanRequestApproval = "media.media.request-approval";
        public const string MediaOrderRequestApproval = "media.mediaorder.request-approval";
        public const string EmployeeLoanNotification = "hris.employeeloan.need-approval";
        public const string EmployeeLoanCoopNotification = "hris.employeeloancoop.need-approval";
        public const string TrainingrequisitionNotification = "hris.trainingrequisition.need-approval";
        public const string TrainingrequisitionParticipant = "hris.trainingrequisition.participant";
        public const string PayrollNotification = "hris.payroll.need-approval";
        public const string DemotionNotification = "hris.demotion.need-approval";
        public const string SanctionNotification = "hris.sanction.need-approval";
        public const string PromotionNotification = "hris.promotion.need-approval";
        public const string ResignationNotification = "hris.Resignation.need-approval";
        public const string MassLeaveNotification = "hris.MassLeave.need-approval";
        public const string OvertimeNotification = "hris.overtime.need-approval";
        public const string LeaveNotification = "hris.leave.need-approval";
        public const string ServiceRequestNotificationApproval = "hris.ServiceRequest.need-approval";
        public const string ServiceRequestNotification = "hris.ServiceRequest.need-notif";
        public const string ServiceRequestNotificationEdit = "hris.ServiceRequest.need-notif-edit";
        public const string FullFillmentNotification = "hris.FullFillment.need-notif";
        public const string FullFillmentNotificationEdit = "hris.FullFillment.need-notif-edit";
        public const string CommentNotification = "hris.Comment.need-notif";
        public const string CommentNotificationEdit = "hris.Comment.need-notif-edit";
        public const string ExitInterviewNotification = "hris.ExitInterview.need-approval";
        public const string ExitInterviewUser = "hris.exitinterview.participant";
        #endregion


    }

    public static class NotificationModuleType
    {
        public const string ModuleFinance = "FINANCE";
        public const string ModuleHRIS = "HRIS";
        public const string ModulePM = "PROJECT MANAGEMENT";
        public const string ModuleMEDIA = "MEDIA";
    }
}
