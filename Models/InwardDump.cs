using System;
using System.Collections.Generic;

namespace SMFG_API_New.Models;

public partial class InwardDump
{
    public int Id { get; set; }

    public string? BranchCode { get; set; }

    public string? BranchName { get; set; }

    public string? CustomerId { get; set; }

    public string? GroupId { get; set; }

    public string? AppRefNo { get; set; }

    public string? Lan { get; set; }

    public string? CustomerName { get; set; }

    public string? Disperseddate { get; set; }

    public string? ProductCode { get; set; }

    public string? ProductName { get; set; }

    public string? LoanAmount { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Podno { get; set; }

    public DateTime? DespatchedDate { get; set; }

    public string? CourierName { get; set; }

    public DateTime? AwbentryDate { get; set; }

    public int? AwbentryBy { get; set; }

    public string? DocumentType { get; set; }

    public string? Status { get; set; }

    public string? FileNo { get; set; }

    public string? CartonNo { get; set; }

    public int? InwardBy { get; set; }

    public DateTime? InwardDate { get; set; }

    public int? AcknowledgeBy { get; set; }

    public DateTime? AcknowledgeDate { get; set; }

    public string? Kyc { get; set; }

    public string? Repaymentsheet { get; set; }

    public string? Dpn { get; set; }

    public string? Mip { get; set; }

    public string? Remark { get; set; }

    public string? RecordType { get; set; }

    public string? ChequeNo1 { get; set; }

    public string? ChequeNo2 { get; set; }

    public string? ChequeNo3 { get; set; }

    public string? ChequeNo4 { get; set; }

    public DateTime? PoddetailsDate { get; set; }

    public int? PoddetailsBy { get; set; }

    public string? ChequeNo5 { get; set; }

    public string? Kyccoapplicant { get; set; }

    public string? IsSync { get; set; }

    public DateTime? SyncDate { get; set; }
}
