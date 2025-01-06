using System;
using System.Collections.Generic;

namespace SMFG_API_New.Models;

public partial class InwardFile
{
    public int Id { get; set; }

    public string? Lan { get; set; }

    public string? DocumentType { get; set; }

    public string? BatchNo { get; set; }

    public int? EntryBy { get; set; }

    public DateTime? EntryDate { get; set; }

    public string? Podno { get; set; }

    public DateTime? AwbentryDate { get; set; }

    public int? AwbentryBy { get; set; }

    public DateTime? AcknowledgeDate { get; set; }

    public int? CourierName { get; set; }

    public DateTime? InwardDate { get; set; }

    public string? Status { get; set; }

    public string? FileStatus { get; set; }

    public int? AcknowledgeBy { get; set; }

    public string? FileNo { get; set; }

    public string? CartonNo { get; set; }

    public string? Repaymentsheet { get; set; }

    public string? Kyc { get; set; }

    public string? Dpn { get; set; }

    public string? Mip { get; set; }

    public string? Kyccoapplicant { get; set; }

    public int? PoddetailsBy { get; set; }

    public DateTime? PoddetailsDate { get; set; }

    public string? RecordType { get; set; }

    public string? Remark { get; set; }

    public string? ChequeNo1 { get; set; }

    public string? ChequeNo2 { get; set; }

    public string? ChequeNo3 { get; set; }

    public string? ChequeNo4 { get; set; }

    public string? ChequeNo5 { get; set; }

    public int? InwardBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}
