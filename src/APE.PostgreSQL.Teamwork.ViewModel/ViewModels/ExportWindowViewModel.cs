// <copyright file="ExportWindowViewModel.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>
using APE.CodeGeneration.Attributes;

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// ViewModel for the <see cref="ExportWindowView"/> which displays the 
    /// diff and undo diff file.
    /// </summary>
    [NotifyProperty(typeof(SQLFileDisplayData), "DiffFile")]
    [NotifyProperty(typeof(SQLFileDisplayData), "UndoDiffFile")]
    [NotifyPropertySupport]
    public partial class ExportWindowViewModel
    {
        public ExportWindowViewModel(SQLFileDisplayData diffFile, SQLFileDisplayData undoDiffFile)
        {
            this.DiffFile = diffFile;
            this.UndoDiffFile = undoDiffFile;
        }
    }
}
