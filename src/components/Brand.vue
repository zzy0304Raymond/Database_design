<template>
    <div class="brand-page">
      <section class="brand-intro">
        <h1>关于 AuctionHub</h1>
        <p>AuctionHub 是全球领先的在线拍卖平台，致力于为用户提供安全、透明、便捷的拍卖体验。我们汇聚了从收藏品到科技产品的各种精品。</p>
      </section>
      
      <section class="brand-mission">
        <h2>我们的使命</h2>
        <p>通过创新技术和全球化市场，我们为每个人提供以最佳价格购买或出售商品的机会。无论您是买家还是卖家，我们都确保交易安全且高效。</p>
      </section>
  
      <section class="brand-values">
        <h2>核心价值</h2>
        <ul>
          <li><strong>诚信：</strong> 每一笔交易都公开透明，确保用户的信任与安全。</li>
          <li><strong>创新：</strong> 持续推动技术革新，提升用户的拍卖体验。</li>
          <li><strong>客户至上：</strong> 客户服务团队时刻待命，确保用户获得最佳支持。</li>
        </ul>
      </section>
  
      <section class="brand-contact">
        <h2>联系我们</h2>
        <p>如有任何疑问或需要帮助，您可以通过以下方式联系我们：</p>
        <p><strong>邮箱：</strong> support@auctionhub.com</p>
        <p><strong>电话：</strong> +123-456-7890</p>
      </section>
    </div>
  </template>
  
  <script>
  export default {
    name: 'Brand'
  };
  </script>
  
  <style scoped>
  .brand-page {
    position: relative;
    padding: 40px 60px;
    text-align: left; /* 左对齐文本 */
    background-image: url('@/assets/images/background.jpg'); /* 背景图片 */
    background-size: cover;
    background-position: center;
    background-attachment: fixed;
    background-repeat: no-repeat;
    font-family: 'Helvetica Neue', Arial, sans-serif;
    color: #333;
    }

    .brand-page::before {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(255, 255, 255, 0.7); /* 添加一个白色的透明遮罩层 */
    z-index: -1;
    }

    .brand-intro, .brand-mission, .brand-values, .brand-contact {
    position: relative;
    margin-bottom: 40px;
    padding: 20px;
    background: rgba(255, 255, 255, 1); /* 使用白色背景和透明度 */
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
  