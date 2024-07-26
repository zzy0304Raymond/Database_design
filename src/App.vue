<template>
  <div id="app">
    <el-menu :default-active="activeIndex" class="el-menu-demo" mode="horizontal" @select="handleSelect">
      <el-menu-item index="1">
        <router-link to="/">Home</router-link>
      </el-menu-item>
      <el-menu-item index="2">
        <router-link to="/login">Login</router-link>
      </el-menu-item>
      <el-menu-item index="3">
        <router-link to="/register">Register</router-link>
      </el-menu-item>
      <el-menu-item index="4">
        <router-link to="/profile">Profile</router-link>
      </el-menu-item>
      <el-menu-item index="5">
        <router-link to="/inbox">Inbox</router-link>
      </el-menu-item>
      <el-menu-item index="6">
        <router-link to="/admin">Admin</router-link>
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
    };
  },
  methods: {
    handleSelect(key, keyPath) {
      console.log(key, keyPath);
    },
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
  margin-top: 60px;
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
