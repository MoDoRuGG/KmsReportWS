using System;

[Serializable]
public class ApprovalInfoDto
{
    public string Direction { get; set; }
    public string DirectionName { get; set; }
    public string EmployeeName { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public bool IsApproved { get; set; }
}