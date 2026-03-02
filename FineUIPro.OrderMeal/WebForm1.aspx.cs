using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FineUIPro.OrderMeal
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder table = new StringBuilder();
            table.Append("<span style='color:#2E75B6'>各位同仁好,</span><br/><br/>");
            table.Append("<span style='color:#2E75B6'>員工團膳將於11/14(一) 更換新團膳公司進駐餐廳供膳 , 請同仁用餐後回饋餐後感,  以期快速調整餐點，讓大家吃的美味又開心， 謝謝。</span><br/><br/>");
            table.Append("<span>1) 請於每日16:00前，進入訂餐系統選訂明天的午餐(或取消)以備足量供餐。為確保您訂餐完成，建議您登出再請重新登入確認。<br/> 訂餐系統 http://192.168.1.41/T100/jing/OrderMeal/login.aspx </span><br/><br/>");
            table.Append("<span>2) 臨時請假不用餐，請立即取消餐(當日時限08:40前)，或請釘釘Angela分機3267協助，以節省資源不造成餐食之耗資浪費,謝謝！</span><br/><br/>");
            table.Append("<span>3) 不用餐的同仁，請勾選 [ 不訂餐 ]。另外若您未勾不訂餐，但收到不訂餐通知，就代表訂餐失效，請重新訂餐。</span><br/><br/>");
            table.Append("<span><< 大家防疫確實做好，保護您我一切安好 >></span><br/><br/>");
            //table.Append("TO : 各位同仁<br/><br/>");
            //table.Append("提醒您，明天訂餐好了嗎 ?<br/><br/>");
            //table.Append("<span>1) 請於每日16:00前，進入訂餐系統選訂明天的午餐，以利準備您的美味餐點。為確認有效訂餐訊息，請您登出後再次重新登入確認哦。</span><br/><br/>");
            //table.Append("<span>2) 臨時無法用餐之情況，請立即上線取消餐(時限08:40前)，或釘釘Angela分機3267協助，以節省資源不造成餐食之耗資浪費,謝謝！</span><br/><br/>");
            //table.Append("<span>3) 用餐分流：Ａ組雙週：業務一處，研發一處，資材處。</span><br/>");
            //table.Append("<span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Ｂ組單週：業務二處， 研發二處，品保部，會計部，資訊處，管理處。</span><br/><br/>");
            //table.Append("<span>4) 不用餐的同仁，亦請勾選 [ 不訂餐 ]。</span><br/><br/>");
            //table.Append("<span><< 大家防疫確實做好，保護您我一切安好 >></span><br/><br/>");
            Response.Write(table.ToString());
        }
    }
}