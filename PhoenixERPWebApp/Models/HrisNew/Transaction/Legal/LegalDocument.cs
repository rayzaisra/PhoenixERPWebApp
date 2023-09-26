using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoenixERPWebApp.Models
{
    public class LegalDocument : HrisEntityBase
    {
        public LegalDocument()
        {
            LegalDocumentCounterpart = new LegalDocumentCounterpart();
        }

        [DisplayName("Id")]
        public string Id { get; set; }

        [DisplayName("Category")]
        public string Category { get; set; }

        [DisplayName("Category")]
        public string LegalCategoryId { get; set; }

        [DisplayName("Document Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DocumentDate { get; set; }

        [DisplayName("Expired Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiredDate { get; set; }

        [DisplayName("Upload Files")]
        public string FileUpload { get; set; }

        [DisplayName("Label")]
        public string Label { get; set; }

        [DisplayName("Remarks")]
        public string Remarks { get; set; }

        public LegalDocumentCounterpart LegalDocumentCounterpart { get; set; }

        [DisplayName("Files")]
        public string FileUploadName { get; set; }

        [DisplayName("Category")]
        public string LegalCategoryName { get; set; }

        [DisplayName("Location Name")]
        public string LocationName { get; set; }

        [DisplayName("Division")]
        public string DivisionName { get; set; }
    }

    public class LegalDocumentSearch
    {
        [DisplayName("Category")]
        public string Category { get; set; }

        [DisplayName("Category")]
        public string LegalCategoryId { get; set; }

        [DisplayName("Counterpart Name")]
        public string CounterpartId { get; set; }

    }

    public class DashboardSearch : LegalDocumentSearch
    {

        [DisplayName("Division")]
        public string DivisionId { get; set; }

        [DisplayName("Location")]
        public string LocationId { get; set; }

        [DisplayName("Expired Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiredDate { get; set; }
    }     
}