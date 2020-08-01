$("#form1").validate({
    rules: {
        UserName: { required: true, maxlength: 15 },
        UserEmile: { required: true, email: true },
        UserPwd: { required: true, checkPwd: true },
        UserEmile1: { required: true, email: true },
        UserPwd1: { required: true, checkPwd: true },
        UserPwd1_1: { required: true, equalTo: "#UserPwd1" },
        validateCode: { required: true },
        shenfenzhen: { required: true, minlength: 15 },
        Name: { required: true },
        Addresss2: { required: true },
        Phone: { required: true },
        JuBaoName: { required: true, maxlength: 15 },
        UserId2: { required: true }, 
        Wuliu: { required: true },
        Photos: { required: true },
        Info: { required: true },
        a_1: { required: true },
        GoodsId: { required: true },
        TypeName: { required: true, maxlength: 15 },
        GoodsTypeBId: { required: true },
        HuiFu: { required: true },
        JuBao_body: { required: true },
        Addresss1_se: { required: true },
        Addresss1_shi: { required: true },
        yanzhen:{ required: true}
    },
    messages: {
        UserName: { required: "昵称必填！", maxlength: "昵称不得大于15位！" },
        UserEmile: { required: "邮箱必填!", email: "请输入标准邮箱！"  },
        UserPwd: { required: "密码必填!", checkPwd: "密码组成：6-12位的字母数字组成！"  },
        UserEmile1: { required: "邮箱必填!", email: "请输入标准邮箱！" },
        UserPwd1: { required: "密码必填!", checkPwd: "密码组成：6-12位的字母数字组成！" },
        UserPwd1_1: { required: "请重新输入密码！", equalTo: "与密码输入不一致！" },
        validateCode: { required: "请输入验证码！" },
        shenfenzhen: { required: "请输入身份证号！", minlength: "号码不得小于15位！" },
        Name: { required: "此项为必填项！" },
        Addresss2: { required: "请输入详细收获地址！" },
        Phone: { required: "请输入电话号码！" },
        JuBaoName: { required: "请输入举报标题！", maxlength: "标题不得大于15位！" },
        UserId2: { required: "被举报用户id不能为空！" },
        Wuliu: { required: "请输入物流单号！" },
        Phone: { required: "请选择图片！" },
        Info: { required: "请输入描述信息！" },
        a_1: { required: "请输入跳转地址！" },
        GoodsId: { required: "请输入推广商品id！" },
        TypeName: { required: "请输入分类名称！", maxlength: "分类名称不得大于15位！"},
        GoodsTypeBId: { required: "请输入分类父id！" },
        HuiFu: { required: "此项为必填项！" },
        JuBao_body: { required: "此项为必填项！" },
        Addresss1_se: { required: "此项为必填项！" }, 
        Addresss1_shi: { required: "此项为必填项！" },
        yanzhen: { required: "请输入邮箱接收的验证码！" }
    }
})

$("#form2").validate({
    rules: {
        UserEmile: { required: true, email: true },
        UserPwd: { required: true, checkPwd: true },
        UserName1: { required: true, maxlength: 20 },
        Newpwd: { required: true, checkPwd: true },
        Newpwd2: { required: true, equalTo: "#Newpwd" },
        Name: { required: true },
        yanzhen: { required: true },
        Addresss2: { required: true },
        Phone: { required: true },
        JuBaoName: { required: true, maxlength: 15 },
        JuBao_body: { required: true },
        Addresss1_se: { required: true },
        Addresss1_shi: { required: true }
    },
    messages: {
        UserEmile: { required: "邮箱必填", Email: "请输入标准邮箱" },
        UserPwd: { required: "密码必填", checkPwd: "密码组成：6-12位的字母数字组成" },
        UserName1: { required: true, maxlength: "字符串长度最多20个字" },
        Newpwd: { required: "密码必填", checkPwd: "密码组成：6-12位的字母数字组成" },
        Newpwd2: { required: "重复密码必填", equalTo: "两次密码输入不一致" },
        yanzhen: { required: "请输入验证码！" },
        Name: { required: "请输入收货人姓名！" },
        Addresss2: { required: "请输入详细收获地址！" },
        Phone: { required: "请输入电话号码！" },
        JuBaoName: { required: "请输入反馈标题！", maxlength: "标题不得大于15位" },
        JuBao_body: { required: "此项为必填项！" },
        Addresss1_se: { required: "此项为必填项！" },
        Addresss1_shi: { required: "此项为必填项！" }
    }
})
$("#bxForm").validate({
    rules: {
        Name: { required: true },
        GoodsaddressInfo: { required: true },
        Ipone: { required: true },
        Price: { required: true },
        Info: { required: true },
        rmb: { required: true, range:[10, 999], number: true}
    },
    messages: {
        Name: { required: "请输入收货人姓名！" },
        GoodsaddressInfo: { required: "请输入详细交易地点！" },
        Ipone: { required: "请输入电话号码！" },
        Price: { required: "请输入价格啊！" },
        Info: { required: "请输入你与TA的故事！" },
        rmb: { required: "请输入充值金额", range: "请输入10-999元", number: "请输入数字" }
    }
})


//后台使用的js





$.validator.addMethod("checkPwd", function (value, e) {
    var reg = /^[A-Za-z0-9]{6,12}$/;
    return reg.test(value);
})

//$.validator.addMethod("Email", function (value, e) {
//    var reg = /^([a-zA-Z0-9_]+)@@(([a-zA-Z0-9]+)\.){1,2}[a-z]{2,3}$/;
//    return reg.test(value);
//})


