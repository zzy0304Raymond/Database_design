<template>
  <div class="home">
    <h1>AuctionHub</h1>

    <!-- 折扣推荐模块 -->
    <div class="discount-section">
      <h2>今日特惠</h2>
      <el-carousel :interval="4000" arrow="always">
        <el-carousel-item v-for="(image, index) in localImages" :key="index">
          <div class="carousel-background" :style="{ backgroundImage: `url(${image})` }"></div>
        </el-carousel-item>
      </el-carousel>
    </div>


    <!-- 搜索栏和过滤器 -->
    <div class="search-filter-bar">
      <!-- 将 v-model 改为 searchQueryInput，这里用于暂存用户输入 -->
      <el-input v-model="searchQueryInput" placeholder="Search items..." class="search-bar" clearable>
        <template #append>
          <!-- 点击搜索按钮时触发 searchItems 方法 -->
          <el-button icon="el-icon-search" @click="searchItems">Search</el-button>
        </template>
      </el-input>
      
      <!-- 分类过滤器 -->
      <el-select v-model="selectedCategory" placeholder="Select Category" class="filter-bar">
        <el-option label="All Categories" value=""></el-option>
        <el-option label="painting" value="painting"></el-option>
        <el-option label="jewlry" value="jewlry"></el-option>
        <el-option label="watch" value="watch"></el-option>
        <el-option label="antique" value="antique"></el-option>
      </el-select>
    </div>




    <!-- 拍卖物品列表 -->
    <el-row :gutter="20" class="auction-list">
      <!-- 使用 v-for 渲染每一个拍卖物品 -->
      <el-col :span="8" v-for="item in paginatedItems" :key="item.id">
        <el-card class="auction-item" shadow="hover">
          <!-- 物品图片 -->
          <img :src="item.imageUrl" alt="item.name" class="item-image" />
          <h2>{{ item.name }}</h2>
          <p>Starting Bid: ${{ item.startingBid }}</p>
          <!-- 查看详情按钮 -->
          <el-button type="primary" @click="viewItem(item.id)">View Details</el-button>
        </el-card>
      </el-col>
    </el-row>

    <!-- 翻页控件 -->
    <el-pagination @current-change="handlePageChange" :current-page="currentPage" :page-size="itemsPerPage"
      layout="prev, pager, next" :total="filteredItems.length" class="pagination"></el-pagination>

    <!-- 品牌介绍模块 -->
    <div class="brand-intro">
      <h2>关于AuctionHub</h2>
      <p>AuctionHub 是全球领先的在线拍卖平台，我们致力于为用户提供可靠、安全的在线拍卖体验。通过先进的技术和广泛的市场网络，我们确保每一位用户都能轻松地找到心仪的商品，并通过竞价获得最优惠的交易。</p>
      <p>我们的使命是重新定义在线拍卖，以高效、透明和用户为核心的方式连接买家和卖家。无论是珍藏品、艺术品，还是高科技电子产品，我们都为用户提供丰富多样的拍卖选择。</p>
      <el-button type="primary" @click="learnMore">了解更多</el-button>
    </div>

    <!-- 新增按钮，前往大厅聊天系统 -->
    <el-button type="success" @click="goToChat">前往大厅聊天系统</el-button>

  </div>
</template>

<script>
import axios from 'axios';

const BACKEND_BASE_URL = import.meta.env.VITE_API_BACKEND_BASE_URL;

import image1 from '@/assets/images/image1.jpg';
import image2 from '@/assets/images/image2.jpg';
import image3 from '@/assets/images/image3.jpg';

export default {
  name: 'Home',
  data() {
    return {
      localImages: [
        image1,  // 使用导入的图片
        image2,
        image3,
      ],
      auctionItems: [],
      searchQueryInput: '',  // 存储用户输入的搜索关键字
      searchQuery: '',
      selectedCategory: '',
      currentPage: 1,
      itemsPerPage: 6,
      searchResults: [],
    };
  },
  watch: {
    selectedCategory() {
      console.log('Category changed:', this.selectedCategory);  // 调试分类选择变化
      this.currentPage = 1; // 当分类改变时重置分页
    },
  },

  computed: {
    filteredItems() {
      let items = this.auctionItems;

      // 输出所有物品列表进行调试
      console.log('All auction items:', items);

      // 搜索关键字过滤
      if (this.searchQuery) {
        console.log('Filtering by search query:', this.searchQuery);  // 调试搜索关键字
        items = items.filter(item => 
          item.name.toLowerCase().includes(this.searchQuery.toLowerCase())
        );
      }

      // 分类过滤，确保当选择 "All Categories" 时不过滤
      if (this.selectedCategory && this.selectedCategory !== '') {
        console.log('Filtering by category:', this.selectedCategory);  // 调试分类过滤
        items = items.filter(item => 
          item.category.toLowerCase() === this.selectedCategory.toLowerCase()
        );
      }

      console.log('Filtered items:', items);  // 输出过滤后的物品列表
      return items;  // 返回最终过滤后的结果
    },
    paginatedItems() {
      const start = (this.currentPage - 1) * this.itemsPerPage;
      const end = start + this.itemsPerPage;
      return this.filteredItems.slice(start, end);
    },
  },

  methods: {
    async fetchAuctionItems() {
      try {
        const response = await axios.get(`${BACKEND_BASE_URL}/auction-items`);
        this.auctionItems = response.data;  // 这里接收的是返回的物品列表
        console.log('Fetched auction items:', this.auctionItems);  // 调试获取的拍卖物品
      } catch (error) {
        console.error('Error fetching auction items:', error);
      }
    },
    async fetchDiscountItems() {
      try {
        // const response = await axios.get(`${BACKEND_BASE_URL}/discount-items`);
        this.discountItems = response.data.items;
      } catch (error) {
        console.error('Error fetching discount items:', error);
      }
    },
    viewItem(id) {
      console.log('Viewing item with ID:', id);  // 调试查看物品详情
      this.$router.push({ name: 'ItemDetail', params: { id } });
    },
    // async searchItems() {
    //   try {
    //     const response = await axios.get(`${BACKEND_BASE_URL}/search-items`, {
    //       params: { query: this.searchQuery }
    //     });
    //     this.searchResults = response.data.items;
    //     this.currentPage = 1; // 重置分页
    //   } catch (error) {
    //     console.error('Error searching items:', error);
    //     this.searchResults = [];
    //   }
    // },
    // async filterByCategory() {
    //   await this.fetchAuctionItems();
    // },
    async searchItems() {
      this.searchQuery = this.searchQueryInput;  // 将输入框的内容赋值给 searchQuery
      this.currentPage = 1; // 重置分页
    },
    handlePageChange(val) {
      this.currentPage = val;
      // this.fetchAuctionItems();
      console.log('Page changed to:', this.currentPage);  // 调试页码变化
    },
    learnMore() {
      this.$router.push('/brand');
    },
    goToChat() {
      this.$router.push('/chat');  // 假设聊天系统路由是 "/chat"
    },
  },
  mounted() {
    this.fetchAuctionItems();
    this.fetchDiscountItems();
  },
};
</script>

<style scoped>
.home {
  padding: 20px;
  display: flex; /* 使用 Flex 布局 */
  flex-direction: column; /* 垂直排列元素 */
  align-items: center; /* 水平居中 */
  background: linear-gradient(135deg, #f5f7fa 0%, #eef2f6 100%); /* 渐变背景 */
  display: flex;
  /* 使用 Flex 布局 */
  flex-direction: column;
  /* 垂直排列元素 */
  align-items: center;
  /* 水平居中 */
  background: #f5f7fa;
  /* 设置背景颜色 */
}

.navbar {
  width: 100%;
  background-color: #056bd2; /* 导航栏背景色 */
  margin-bottom: 20px; /* 与下方内容间距 */
  background-color: #2c3e50;
  /* 导航栏背景色 */
  margin-bottom: 20px;
  /* 与下方内容间距 */
}

.nav-menu {
  background-color: #2288ed; /* 菜单背景色 */
  color: white; /* 菜单文字颜色 */
  background-color: #2c3e50;
  /* 菜单背景色 */
  color: white;
  /* 菜单文字颜色 */
}

.page-title {
  font-size: 2.5rem;
  /* 标题字体大小 */
  font-weight: bold;
  /* 标题字体加粗 */
  margin: 20px 0;
  /* 上下间距 */
  color: #34495e;
  /* 标题颜色 */
  text-align: center;
  /* 居中对齐 */
}

.search-filter-bar {
  display: flex;
  /* 使用 Flex 布局 */
  width: 80%;
  justify-content: space-between;
  /* 两端对齐 */
  margin-bottom: 30px;
  /* 与下方内容间距 */
}

.search-bar,
.filter-bar {
  flex: 1;
  /* 平分 Flex 容器 */
  margin: 0 10px;
  /* 左右间距 */
  max-width: 300px;
  /* 最大宽度 */
}

.auction-list {
  width: 100%;
  display: flex;
  /* 使用 Flex 布局 */
  flex-wrap: wrap;
  /* 换行显示 */
  justify-content: flex-start;
  /* 居中对齐 */
}

.auction-item {
  margin: 10px; /* 四周间距 */
  padding: 20px; /* 内边距 */
  border-radius: 10px; /* 圆角 */
  transition: box-shadow 0.3s ease; /* 阴影过渡效果 */
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
  background-color: #fff;
  margin: 10px;
  /* 四周间距 */
  padding: 20px;
  /* 内边距 */
  border-radius: 10px;
  /* 圆角 */
  transition: box-shadow 0.3s ease;
  /* 阴影过渡效果 */
}

.auction-item:hover {
  transform: translateY(-5px); /* 悬停时的轻微上移效果 */
  box-shadow: 0 8px 20px rgba(0, 0, 0, 0.2); /* 加强阴影效果 */
  transition: transform 0.3s ease, box-shadow 0.3s ease;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  /* 悬停时阴影效果 */
}

.item-image {
  width: 100%; /* 图片宽度自适应 */
  max-height: 200px; /* 图片最大高度 */
  object-fit: cover; /* 保持图片比例 */
  border-radius: 8px; /* 图片圆角 */
  margin-bottom: 10px; /* 图片与下方文字的间距 */
  transition: transform 0.3s ease; /* 添加图片缩放动画 */
}

.item-image:hover {
  transform: scale(1.05); /* 悬停时图片轻微放大 */
}

.el-button {
  transition: transform 0.2s ease, background-color 0.3s ease;
}

.el-button:hover {
  transform: scale(1.05); /* 按钮悬停时轻微放大 */
  background-color: #2c3e50; /* 改变背景色 */
}

.pagination {
  margin-top: 20px;
  /* 与上方内容间距 */
  display: flex;
  /* 使用 Flex 布局 */
  justify-content: center;
  /* 居中对齐 */
}

.discount-image {
  width: 100%; /* 让图片自适应宽度 */
  height: 500px; 
  /* 固定图片高度 */
  object-fit: cover; /* 保持图片的宽高比，剪裁溢出的部分 */
  border-radius: 8px; /* 可选：给图片加上圆角 */
}

.discount-section {
  width: 100%;
  margin: 20px 0;
  padding: 20px 0;
  background-color: #fff;
  text-align: center;
}

.el-carousel {
  height: 300px; /* 轮播图组件的高度与图片相匹配 */
  margin: 0 auto;
  background-position: center;
  background-size: contain;
}

.carousel-background {
  display: flex;
  justify-content: center;
  align-items: center;
  width: 70%; /* 让背景图片适应宽度 */
  height: 300px; /* 设置轮播图的高度 */
  background-size: cover; /* 保证图片不会被裁剪，适应容器 */
  background-position: center; /* 图片居中显示 */
  background-repeat: no-repeat; /* 禁止背景图片重复 */
  border-radius: 8px; /* 可选：给背景图片加上圆角 */
  overflow: visible; /* 显示超出容器的部分 */
  padding-left: 500px; /* 左侧内边距 */
}

.brand-intro {
  text-align: center;
  padding: 20px;
  background-color: #f9f9f9;
  width: 100%;
  /* 占满页面宽度 */
  margin: 20px 0;
  box-sizing: border-box;
  /* 包含 padding 在内的宽度 */
}

.dialog-image {
  width: 100%;
  max-height: 300px;
  object-fit: cover;
  border-radius: 8px;
  margin-bottom: 20px;
}

</style>