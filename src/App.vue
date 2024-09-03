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
        { index: '8', name: 'SellItem', route: '/sell' },
      ],
    };
  },
  methods: {
    handleSelect(key, keyPath) {
      console.log(key, keyPath);
    },
  },
  created() {
    this.activeIndex = this.menuItems.find(item => item.route === this.$route.path)?.index || '1';
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
