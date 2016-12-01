using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using APE.CodeGeneration.Attributes;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;
using StructureMap.AutoMocking.Moq;

namespace APE.PostgreSQL.Teamwork.TestHelper
{
    // APE.CodeGeneration.Attribute [Disposable]
    public partial class Test<T>
         : System.IDisposable
    {
        // APE CodeGeneration Generated Code
        //ncrunch: no coverage start

        //--------------------------------------------------------------------------------
        // generated code for Disposable
        //--------------------------------------------------------------------------------

        private bool isDisposed = false;

        /// <summary>
        /// Performs application-defined tasks associated with
        /// freeing, releasing or resetting unmanaged resources.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "False positive. IDisposable is implemented via partial method.")]
        public void Dispose()
        {
            if (this.isDisposed)
                return;

            this.Dispose(true);
            System.GC.SuppressFinalize(this); //do not execute 
            this.isDisposed = true;
        }

        /// <summary>
        /// Release unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="threadSpecificCleanup"><c>true</c> means that the method is calld by the owning thread;
        /// <c>false</c> means it is called by the finalizer thread
        /// resources.</param>
        partial void Dispose(bool threadSpecificCleanup);

        ~Test()
        {
            if (this.isDisposed)
                return;

            this.Dispose(false);
            this.isDisposed = true;
        }

        //ncrunch: no coverage end
    }
}
