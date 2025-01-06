using System;
using System.Collections.Generic;

namespace SMFG_API_New.Models;

public partial class SysRole
{
    public int Id { get; set; }

    public string? RoleName { get; set; }

    public string? Remarks { get; set; }

    public string? PageId { get; set; }

    public string? PageRights { get; set; }

    public string? TaggingRights { get; set; }
}
