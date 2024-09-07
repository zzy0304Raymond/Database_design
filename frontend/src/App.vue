<template>
  <div id="app">
    <el-menu :default-active="activeIndex" class="el-menu-demo" mode="horizontal" @select="handleSelect">
      <el-menu-item v-for="item in menuItems" :key="item.index" :index="item.index">
        <router-link :to="item.route">{{ item.name }}</router-link>
      </el-menu-item>
    </el-menu>
    <router-view></router-view>
  </div>
</template>

<script>
import axios from 'axios';
const BACKEND_BASE_URL = import.meta.env.VITE_API_BACKEND_BASE_URL;

export default {
  name: 'App',
  data() {
    return {
      activeIndex: '1',
      menuItems: [
        { index: '1', name: 'Home', route: '/' },
        { index: '2', name: 'Login', route: '/login' },
        { index: '3', name: 'Register', route: '/register' },
        { index: '4', name: 'Profile', route: '/profile' },
        { index: '5', name: 'Inbox', route: '/inbox' },
        { index: '6', name: 'Admin', route: '/admin' },
        { index: '7', name: 'UserManual', route: '/manual' },
        // { index: '8', name: 'SellItem', route: '/sell' },
      ],
      checkInterval: null, // 定时器ID
      auctionItemId: null, // 存储返回的ID
    };
  },
  methods: {
    handleSelect(key, keyPath) {
      console.log(key, keyPath);
    },
    // 调用第一个接口，检查条件
    checkCondition() {
      axios.get(`${BACKEND_BASE_URL}/auction-items/confirm`) // 调用第一个API
        .then(response => {
          if (response.data.itemId > 0) { // 判断条件是否满足
            console.log('条件满足，进入第二个API...');
            console.log(response.data.itemId);
            this.auctionItemId = response.data.itemId; // 将返回的数据存储为id
            this.callAnotherApi(this.auctionItemId); // 调用第二个API，传递id参数
          } else {
            console.log(response.data.itemId);
            console.log('条件未满足，继续循环，返回的ID:', this.auctionItemId);
          }
        })
        .catch(error => {
          console.error('接口调用失败:', error);
        });
    },

    // 循环检查条件
    startChecking() {
      this.checkInterval = setInterval(() => {
        this.checkCondition(); // 每次定时调用 checkCondition
      }, 5000); // 每5秒调用一次接口
    },

    // 调用第二个接口
    callAnotherApi(id) {
      axios.delete(`${BACKEND_BASE_URL}/auction-items/${id}`) // 使用传递过来的ID作为参数
        .then(response => {
          console.log('第二个接口调用成功:', response.data);
        })
        .catch(error => {
          console.error('调用第二个接口失败:', error);
        });
    },
  },
  created() {
    this.activeIndex = this.menuItems.find(item => item.route === this.$route.path)?.index || '1';
    this.startChecking(); // 在组件加载时开始循环检查
  },
  beforeDestroy() {
    // 确保组件销毁时停止定时器，避免内存泄漏
    if (this.checkInterval) {
      clearInterval(this.checkInterval);
    }
  },
};
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

.el-menu-demo {
  background-color: #333;
  color: #fff;
  border: none;
  margin-bottom: 20px;
}

.el-menu-demo .el-menu-item {
  color: #fff;
}

.el-menu-demo .el-menu-item:hover {
  background-color: #444;
}

.el-menu-demo .el-menu-item.is-active {
  background-color: #666;
}

nav a.router-link-exact-active {
  font-weight: bold;
  color: #35495e;
}
</style>
