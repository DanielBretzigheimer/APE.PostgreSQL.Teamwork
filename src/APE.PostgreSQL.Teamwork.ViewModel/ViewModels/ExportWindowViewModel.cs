// <copyright file="ExportWindowViewModel.cs" company="APE Engineering GmbH">Copyright (c) APE Engineering GmbH. All rights reserved.</copyright>

namespace APE.PostgreSQL.Teamwork.ViewModel
{
    /// <summary>
    /// ViewModel for the ExportWindowView which displays the diff and undo diff file.
    /// </summary>
    public partial class ExportWindowViewModel
    {
        public ExportWindowViewModel(SQLFileDisplayData diffFile, SQLFileDisplayData undoDiffFile)
        {
            this.DiffFile = diffFile;
            this.UndoDiffFile = undoDiffFile;
        }
    }
}
