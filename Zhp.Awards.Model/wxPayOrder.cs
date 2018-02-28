using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zhp.Awards.Model
{
    public class wxPayOrder
    {
        //公众账号ID
        public string appid { get; set; }
        //商户号
        public string mch_id { get; set; }
        //设备号
        public string device_info { get; set; }
        //随机字符串
        public string nonce_str { get; set; }
        //签名
        public string sign { get; set; }
        //签名类型
        public string sign_type { get; set; }
        //商品描述
        public string body { get; set; }
        //商品详情
        public string detail { get; set; }
        //附加数据
        public string attach { get; set; }
        //商户订单号
        public string out_trade_no { get; set; }
        //标价币种
        public string fee_type { get; set; }
        //标价金额
        public string total_fee { get; set; }
        //终端IP
        public string spbill_create_ip { get; set; }
        //交易起始时间
        public string time_start { get; set; }
        //交易结束时间
        public string time_expire { get; set; }
        //订单优惠标记
        public string goods_tag { get; set; }
        //通知地址
        public string notify_url { get; set; }
        //交易类型
        public string trade_type { get; set; }
        //商品ID
        public string product_id { get; set; }
        //指定支付方式
        public string limit_pay { get; set; }
        //用户标识
        public string openid { get; set; }

        //        <xml>
        //   <appid>wx6953deeefe22a83b</appid>
        //   <attach>支付测试</attach>
        //   <body>JSAPI支付测试</body>
        //   <mch_id>1267684801</mch_id>
        //   <detail><![CDATA[{ "goods_detail":[ { "goods_id":"iphone6s_16G", "wxpay_goods_id":"1001", "goods_name":"iPhone6s 16G", "quantity":1, "price":528800, "goods_category":"123456", "body":"苹果手机" }, { "goods_id":"iphone6s_32G", "wxpay_goods_id":"1002", "goods_name":"iPhone6s 32G", "quantity":1, "price":608800, "goods_category":"123789", "body":"苹果手机" } ] }]]></detail>
        //   <nonce_str>1add1a30ac87aa2db72f57a2375d8fec</nonce_str>
        //   <notify_url>http://wxpay.wxutil.com/pub_v2/pay/notify.v2.php</notify_url>
        //   <openid>oUpF8uMuAJO_M2pxb1Q9zNjWeS6o</openid>
        //   <out_trade_no>1415659990</out_trade_no>
        //   <spbill_create_ip>14.23.150.211</spbill_create_ip>
        //   <total_fee>0.1</total_fee>
        //   <trade_type>JSAPI</trade_type>
        //   <sign>0CB01533B8C1EF103065174F50BCA001</sign>
        //</xml>
    }
}
