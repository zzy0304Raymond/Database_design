<template>
  <div class="user-manual-page">
    <section class="manual-intro">
      <h1>使用手册</h1>
      <p>欢迎使用 AuctionHub 在线拍卖平台！本手册将为您提供使用该平台的详细指南。</p>
    </section>

    <section class="manual-quickstart">
      <h2>快速入门</h2>
      <h3>如何注册</h3>
      <ul>
        <li>点击页面右上角的 <strong>注册</strong> 按钮。</li>
        <li>填写用户名、邮箱和密码，点击 <strong>注册</strong>。</li>
        <li>注册成功后，系统将引导您进入登录页面。</li>
      </ul>

      <h3>如何登录</h3>
      <ul>
        <li>点击页面右上角的 <strong>登录</strong> 按钮。</li>
        <li>输入注册时使用的邮箱和密码，点击 <strong>登录</strong>。</li>
        <li>登录成功后，您将进入个人主页。</li>
      </ul>
    </section>

    <section class="manual-features">
      <h2>功能介绍</h2>
      <h3>搜索商品</h3>
      <p>在主页的搜索栏中输入您感兴趣的商品名称，点击搜索按钮，即可查看相关商品。</p>

      <h3>浏览商品</h3>
      <p>您可以浏览平台中的各类商品，查看详细的描述、起拍价和当前竞拍价格。</p>

      <h3>参与竞拍</h3>
      <ul>
        <li>进入商品详情页面，查看当前拍卖价格。</li>
        <li>输入您想要的出价金额，点击 <strong>出价</strong> 按钮进行竞拍。</li>
      </ul>
    </section>

    <section class="manual-contact">
      <h2>联系我们</h2>
      <p>如有任何问题，请通过以下方式联系我们：</p>
      <p><strong>邮箱：</strong> support@auctionhub.com</p>
      <p><strong>电话：</strong> +1 800 123 4567</p>
    </section>
  </div>
</template>

<script>
export default {
  name: 'UserManual',
};
</script>

<style scoped>
.user-manual-page {
  position: relative;
  padding: 40px 60px;
  text-align: left;
  background-image: url('@/assets/images/background.jpg'); /* 使用背景图片 */
  background-size: cover;
  background-position: center;
  background-position-y: 43px;
  background-attachment: fixed;
  background-repeat: no-repeat;
  font-family: 'Helvetica Neue', Arial, sans-serif;
  color: #333;
  
}



.manual-intro, .manual-quickstart, .manual-features, .manual-contact {
  position: relative;
  margin-bottom: 40px;
  padding: 20px;
  background: rgba(255, 255, 255, 1);
  border-radius: 8px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
}

h1 {
  font-size: 2.5rem;
  font-weight: bold;
  color: #2c3e50;
  margin-bottom: 10px;
}

h2 {
  font-size: 1.8rem;
  font-weight: bold;
  color: #056bd2;
  margin-bottom: 10px;
}

h3 {
  font-size: 1.4rem;
  font-weight: bold;
  margin-bottom: 5px;
  color: #2c3e50;
}

p, ul {
  font-size: 1.1rem;
  line-height: 1.8;
  color: #4a4a4a;
}

ul {
  list-style-type: none;
  padding-left: 0;
}

ul li {
  font-size: 1.1rem;
  color: #34495e;
  margin-bottom: 10px;
  padding-left: 20px;
  position: relative;
}

ul li::before {
  content: '•';
  color: #056bd2;
  position: absolute;
  left: 0;
  font-size: 1.2rem;
  line-height: 1.2;
}

strong {
  color: #056bd2;
}
</style>
