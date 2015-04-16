using Trooper.BusinessOperation2.Interface.OperationResponse;

namespace Trooper.Ui.Mvc.Rabbit.Props
{
    public class MessagesPanelProps : ElementProps
    {
	    private int columns = 1;

        private IResponse operationResponse;

	    public int Columns
	    {
		    get { return this.columns; }
			set 
			{
				if (value == 1 || value == 2 || value == 3 || value == 4 || value == 6)
				{
					this.columns = value;
				}
			}
	    }

        public IResponse OperationResponse
        {
            get
            {
                return this.operationResponse;
            }

            set
            {                
                this.Messages = value == null ? null : value.Messages;
                this.operationResponse = value;
            }
        }
    }
}
