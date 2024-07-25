<template>
    <div class="home">
      <h1>Online Auction</h1>
      <div class="search-bar">
        <input type="text" v-model="searchQuery" placeholder="Search items..." @input="searchItems" />
      </div>
      <div class="filter-bar">
        <select v-model="selectedCategory" @change="filterByCategory">
          <option value="">All Categories</option>
          <option value="Antiques">Antiques</option>
          <option value="Art">Art</option>
          <option value="Electronics">Electronics</option>
          <option value="Jewelry">Jewelry</option>
          <!-- 添加更多分类 -->
        </select>
      </div>
      <div class="auction-list">
        <div class="auction-item" v-for="item in paginatedItems" :key="item.id">
          <h2>{{ item.name }}</h2>
          <img :src="item.imageUrl" alt="item.name" class="item-image" />
          <p>Starting Bid: ${{ item.startingBid }}</p>
          <button @click="viewItem(item.id)">View Details</button>
        </div>
      </div>
      <div class="pagination">
        <button @click="prevPage" :disabled="currentPage === 1">Previous</button>
        <span>Page {{ currentPage }} of {{ totalPages }}</span>
        <button @click="nextPage" :disabled="currentPage === totalPages">Next</button>
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
          // 添加更多假数据
        ],
        searchQuery: '',
        selectedCategory: '',
        currentPage: 1,
        itemsPerPage: 3,
      };
    },
    computed: {
      filteredItems() {
        let items = this.auctionItems;
        if (this.searchQuery) {
          items = items.filter(item => item.name.toLowerCase().includes(this.searchQuery.toLowerCase()));
        }
        if (this.selectedCategory) {
          items = items.filter(item => item.category === this.selectedCategory);
        }
        return items;
      },
      totalPages() {
        return Math.ceil(this.filteredItems.length / this.itemsPerPage);
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
        // 处理搜索逻辑
      },
      filterByCategory() {
        // 处理分类筛选逻辑
      },
      prevPage() {
        if (this.currentPage > 1) {
          this.currentPage--;
        }
      },
      nextPage() {
        if (this.currentPage < this.totalPages) {
          this.currentPage++;
        }
      },
    },
  };
  </script>
  
  <style scoped>
  .home {
    padding: 20px;
  }
  .search-bar,
  .filter-bar {
    margin-bottom: 20px;
  }
  .auction-list {
    display: flex;
    flex-wrap: wrap;
  }
  .auction-item {
    border: 1px solid #ccc;
    margin: 10px;
    padding: 10px;
    width: calc(33% - 40px);
    box-sizing: border-box;
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
  .pagination button {
    margin: 0 10px;
  }
  </style>
  