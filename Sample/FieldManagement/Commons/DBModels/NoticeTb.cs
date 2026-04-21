using System;
using System.Collections.Generic;

namespace FieldManagement.Commons.DBModels;

/// <summary>
/// 공지사항 테이블
/// </summary>
public partial class NoticeTb
{
    public int NoticeSeq { get; set; }

    /// <summary>
    /// 제목
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// 설명
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 작성자
    /// </summary>
    public string CreateUser { get; set; } = null!;

    public DateTime CreateDt { get; set; }
}
