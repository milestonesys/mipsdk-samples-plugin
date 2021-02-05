using VideoOS.Platform.Search;
using VideoOS.Platform.Search.FilterValues;

namespace SCSearchAgent.SCAnimalsSearchAgent.SearchUserControl.WinForms
{
    /// <summary>
    /// Interaction logic for SCAnimalsSearchAreaFilterEditHostControl.xaml
    /// </summary>
    public partial class SCAnimalsSearchAreaFilterEditHostControl : SearchFilterEditControl
    {
        public SCAnimalsSearchAreaFilterEditHostControl()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            base.Init();
            _legacyControl.FilterValue = FilterValue as StringFilterValue;
        }
    }
}
