<template>
  <div class="home">
    <h1>Online Auction</h1>

    <!-- 折扣推荐模块 -->
    <div class="discount-section">
      <h2>今日特惠</h2>
      <el-carousel :interval="4000" arrow="always">
        <el-carousel-item v-for="item in discountItems" :key="item.id">
          <div class="discount-item">
            <img :src="item.image" alt="折扣商品" />
            <h3>{{ item.title }}</h3>
            <p>优惠价: {{ item.price }}</p>
          </div>
        </el-carousel-item>
      </el-carousel>
    </div>

    <!-- 搜索栏和过滤器 -->
    <div class="search-filter-bar">
      <el-input
        v-model="searchQuery"
        placeholder="Search items..."
        class="search-bar"
        clearable
      >
        <template #append>
          <el-button icon="el-icon-search" @click="searchItems">Search</el-button>
        </template>
      </el-input>
      <el-select
        v-model="selectedCategory"
        placeholder="Select Category"
        @change="filterByCategory"
        class="filter-bar"
      >
        <el-option label="All Categories" value=""></el-option>
        <el-option label="Antiques" value="Antiques"></el-option>
        <el-option label="Art" value="Art"></el-option>
        <el-option label="Electronics" value="Electronics"></el-option>
        <el-option label="Jewelry" value="Jewelry"></el-option>
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
    <el-pagination
      @current-change="handlePageChange"
      :current-page="currentPage"
      :page-size="itemsPerPage"
      layout="prev, pager, next"
      :total="filteredItems.length"
      class="pagination"
    ></el-pagination>

    <!-- 品牌介绍模块 -->
    <div class="brand-intro">
      <h2>品牌介绍</h2>
      <p>我们的品牌致力于提供高品质产品，服务全球用户...</p>
      <el-button type="primary" @click="learnMore">了解更多</el-button>
    </div>


  </div>
</template>

<script>
export default {
  name: 'Home',
  data() {
    return {
      auctionItems: [
        { id: 1, name: 'Vintage Clock', startingBid: 100, imageUrl: 'https://via.placeholder.com/150', category: 'Antiques' },
        { id: 2, name: 'Antique Vase', startingBid: 200, imageUrl: 'https://via.placeholder.com/150', category: 'Antiques' },
        { id: 3, name: 'Rare Painting', startingBid: 300, imageUrl: 'https://via.placeholder.com/150', category: 'Art' },
        { id: 4, name: 'Smartphone', startingBid: 400, imageUrl: 'https://via.placeholder.com/150', category: 'Electronics' },
        { id: 5, name: 'Laptop', startingBid: 500, imageUrl: 'https://via.placeholder.com/150', category: 'Electronics' },
        { id: 6, name: 'Diamond Ring', startingBid: 600, imageUrl: 'https://via.placeholder.com/150', category: 'Jewelry' },
        { id: 7, name: 'Gold Necklace', startingBid: 700, imageUrl: 'https://via.placeholder.com/150', category: 'Jewelry' },
        { id: 8, name: 'Antique Book', startingBid: 800, imageUrl: 'https://via.placeholder.com/150', category: 'Antiques' },
        { id: 9, name: 'Oil Painting', startingBid: 900, imageUrl: 'https://via.placeholder.com/150', category: 'Art' },
        { id: 10, name: 'Tablet', startingBid: 1000, imageUrl: 'https://via.placeholder.com/150', category: 'Electronics' },
        { id: 11, name: 'Emerald Bracelet', startingBid: 1100, imageUrl: 'https://via.placeholder.com/150', category: 'Jewelry' },
        { id: 12, name: 'Antique Chair', startingBid: 1200, imageUrl: 'https://via.placeholder.com/150', category: 'Antiques' },
      ],
      searchQuery: '',
      selectedCategory: '',
      currentPage: 1,
      itemsPerPage: 3,
      searchResults: [],
      discountItems: [
        { id: 1, title: 'Discount Item 1', price: '¥50', image: '/path_to_uploaded_image/image.png' },
        { id: 2, title: 'Discount Item 2', price: '¥80', image: '/path_to_uploaded_image/image.png' },
        { id: 3, title: 'Discount Item 3', price: '¥120', image: '/path_to_uploaded_image/image.png' },
      ],
    };
  },
  computed: {
    filteredItems() {
      if (this.searchResults.length > 0) {
        return this.searchResults;
      } else {
        let items = this.auctionItems;
        if (this.selectedCategory) {
          items = items.filter(item => item.category === this.selectedCategory);
        }
        return items;
      }
    },
    paginatedItems() {
      const start = (this.currentPage - 1) * this.itemsPerPage;
      const end = start + this.itemsPerPage;
      return this.filteredItems.slice(start, end);
    },
  },
  methods: {
    viewItem(id) {
      this.$router.push({ name: 'ItemDetail', params: { id } });
    },
    searchItems() {
      if (this.searchQuery) {
        this.searchResults = this.auctionItems.filter(item =>
          item.name.toLowerCase().includes(this.searchQuery.toLowerCase())
        );
      } else {
        this.searchResults = [];
      }
      this.currentPage = 1;
    },
    filterByCategory() {
      // 处理分类筛选逻辑
    },
    handlePageChange(val) {
      this.currentPage = val;
    },
    learnMore() {
      // 跳转到品牌详情页
      this.$router.push('/brand');
    },
  },
};
</script>

<style scoped>
.home {
  padding: 20px;
  display: flex; /* 使用 Flex 布局 */
  flex-direction: column; /* 垂直排列元素 */
  align-items: center; /* 水平居中 */
  background: #f5f7fa; /* 设置背景颜色 */
}

.navbar {
  width: 100%;
  background-color: #2c3e50; /* 导航栏背景色 */
  margin-bottom: 20px; /* 与下方内容间距 */
}

.nav-menu {
  background-color: #2c3e50; /* 菜单背景色 */
  color: white; /* 菜单文字颜色 */
}

.page-title {
  font-size: 2.5rem; /* 标题字体大小 */
  font-weight: bold; /* 标题字体加粗 */
  margin: 20px 0; /* 上下间距 */
  color: #34495e; /* 标题颜色 */
  text-align: center; /* 居中对齐 */
}

.search-filter-bar {
  display: flex; /* 使用 Flex 布局 */
  width: 80%;
  justify-content: space-between; /* 两端对齐 */
  margin-bottom: 30px; /* 与下方内容间距 */
}

.search-bar,
.filter-bar {
  flex: 1; /* 平分 Flex 容器 */
  margin: 0 10px; /* 左右间距 */
  max-width: 300px; /* 最大宽度 */
}

.auction-list {
  width: 100%;
  display: flex; /* 使用 Flex 布局 */
  flex-wrap: wrap; /* 换行显示 */
  justify-content: center; /* 居中对齐 */
}

.auction-item {
  margin: 10px; /* 四周间距 */
  padding: 20px; /* 内边距 */
  border-radius: 10px; /* 圆角 */
  transition: box-shadow 0.3s ease; /* 阴影过渡效果 */
}

.auction-item:hover {
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1); /* 悬停时阴影效果 */
}

.item-image {
  width: 100%; /* 图片宽度自适应 */
  max-height: 200px; /* 图片最大高度 */
  object-fit: cover; /* 保持图片比例 */
  border-radius: 8px; /* 图片圆角 */
  margin-bottom: 10px; /* 图片与下方文字的间距 */
}

.pagination {
  margin-top: 20px; /* 与上方内容间距 */
  display: flex; /* 使用 Flex 布局 */
  justify-content: center; /* 居中对齐 */
}

.discount-section {
  width: 100vw; /* 全屏宽度 */
  margin: 20px 0;
  padding: 20px 0;
  background-color: #fff;
  text-align: center;
  overflow: hidden; /* 防止超出内容 */
}

.discount-item {
  padding: 10px;
  display: flex;
  flex-direction: column;
  align-items: center;
}
.discount-item img {
  width: auto;
  height: 150px; /* 图片高度 */
  border-radius: 8px;
}
.brand-intro {
  text-align: center;
  padding: 20px;
  background-color: #f9f9f9;
  border-radius: 10px;
  margin: 20px 0;
}

</style>