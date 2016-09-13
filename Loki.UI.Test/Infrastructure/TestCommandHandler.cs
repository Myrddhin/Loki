using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Loki.UI.Models;

namespace Loki.UI.Commands
{
   public  class TestCommandHandler : NotifyPropertyChanged
    {
        public int ConfirmCalled { get; private set; }

        public int CanExecuteCalled { get; private set; }

        public int ExecuteCalled { get; private set; }

        private bool canExecuteResult;

        private bool confirmResult;

        public TestCommandHandler(bool canExecuteResult, bool confirmResult)
        {
            this.canExecuteResult = canExecuteResult;
            this.confirmResult = confirmResult;
        }

        public void SetReturnValues(bool canExecuteResult, bool confirmResult)
        {
            this.canExecuteResult = canExecuteResult;
            this.confirmResult = confirmResult;
        }

        public void CanExecute(object sender, CanExecuteCommandEventArgs e)
        {
            e.CanExecute = canExecuteResult;
            CanExecuteCalled++;
        }

        public void Execute(object sender, CommandEventArgs e)
        {
            ExecuteCalled++;
        }

        public bool Confirm(CommandEventArgs e)
        {
            ConfirmCalled++;
            return confirmResult;
        }
    }
}
