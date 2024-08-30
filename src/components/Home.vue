<template>
  <div class="home">
    <h1>Online Auction</h1>
    <div class="search-filter-bar">
      <el-input
        v-model="searchQuery"
        placeholder="Search items..."
        class="search-bar"
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
    <el-row :gutter="20" class="auction-list">
      <el-col :span="8" v-for="item in paginatedItems" :key="item.id">
        <el-card class="auction-item" shadow="hover">
          <h2>{{ item.name }}</h2>
          <img :src="item.imageUrl" alt="item.name" class="item-image" />
          <p>Starting Bid: ${{ item.startingBid }}</p>
          <el-button type="primary" @click="viewItem(item.id)">View Details</el-button>
        </el-card>
      </el-col>
    </el-row>
    <el-pagination
      @current-change="handlePageChange"
      :current-page="currentPage"
      :page-size="itemsPerPage"
      layout="prev, pager, next"
      :total="filteredItems.length"
      class="pagination"
    ></el-pagination>
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
  },
};
</script>

<style scoped>
.home {
  padding: 20px;
}
.search-filter-bar {
  display: flex;
  margin-bottom: 20px;
}
.search-bar {
  flex: 2;
  margin-right: 10px;
}
.filter-bar {
  flex: 1;
  min-width: 150px;
}
.auction-list {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-around;
}
.auction-item {
  margin: 10px;
  padding: 10px;
}
.item-image {
  width: 100%;
  height: auto;
}
.pagination {
  display: flex;
  justify-content: center;
  margin-top: 20px;
}
</style>
