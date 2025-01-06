using System;
using System.Collections.Generic;

namespace SMFG_API_New.Models;

public partial class SysUser
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Userid { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public string? Pwd { get; set; }

    public string? Remarks { get; set; }

    public int? SysRoleId { get; set; }

    public byte[]? LoginPass { get; set; }

    public DateTime? CreationDate { get; set; }

    public int? BranchId { get; set; }

    public string? UserToken { get; set; }
}
