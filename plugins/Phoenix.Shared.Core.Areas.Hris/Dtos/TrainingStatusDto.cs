using System;
using Phoenix.Shared.Core.Parameters;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Phoenix.Shared.Core.Areas.Hris.Dtos
{
    public class TrainingStatusDto
    {
        public const string Draft = "0";
        public const string Open = "1";
        public const string CurrentApproval = "2";
        public const string Approved = "3";
        public const string Reject = "4";
        public const string WaitingBonding = "5";
        public const string SubmittoFinance = "6";
        public const string Training = "7";
        public const string ShaaringSeassion = "8";


        public static string GetStatus(string value)
        {
            string result = "";
            switch (value)
            {
                case Open:
                    result = "Open";
                    break;
                case Draft:
                    result = "Draft";
                    break;
                case CurrentApproval:
                    result = "Review";
                    break;
                case Approved:
                    result = "Approved";
                    break;
                case Reject:
                    result = "Reject";
                    break;
                case WaitingBonding:
                    result = "Waiting Bonding";
                    break;
                case SubmittoFinance:
                    result = "Submit to Finance";
                    break;
                case Training:
                    result = "Training";
                    break;
                case ShaaringSeassion:
                    result = "Shaaring Seassion";
                    break;
                default:
                    result = "Draft";
                    break;
            }
            return result;
        }

    }
}