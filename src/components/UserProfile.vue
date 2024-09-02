<template>
  <div class="user-profile" v-if="isAuthenticated"> <!-- 只有登录后才显示 -->
    <!-- 用户资料卡片 -->
    <el-card class="profile-card">
      <div class="profile-header">
        <!-- 用户头像 -->
        <el-avatar :src="user.avatar" size="large"></el-avatar>
        <div class="profile-info">
          <h1>User Profile</h1>
          <!-- 编辑资料按钮，点击时打开编辑对话框 -->
          <el-button type="primary" @click="openEditDialog">Edit Profile</el-button>
        </div>
      </div>
      <div class="user-info">
        <!-- 骨架屏组件，加载时显示占位符 -->
        <el-skeleton :loading="loading" animated>
          <!-- 骨架屏占位模板 -->
          <template #template>
            <el-skeleton-item variant="text"></el-skeleton-item>
            <el-skeleton-item variant="text"></el-skeleton-item>
          </template>
          <!-- 加载完成后显示的用户信息 -->
          <template #default>
            <p>Username: {{ user.username }}</p>
            <p>Email: {{ user.email }}</p>
            <!-- 添加更多用户信息 -->
            <p>Location: {{ user.location }}</p>
            <p>Joined: {{ user.joinedDate }}</p>
          </template>
        </el-skeleton>
      </div>
    </el-card>

    <!-- 竞价历史卡片 -->
    <el-card class="bidding-history-card">
      <h2>Bidding History</h2>
      <!-- 骨架屏组件，加载时显示占位符 -->
      <el-skeleton :loading="loading" animated count="5">
        <!-- 骨架屏占位模板 -->
        <template #template>
          <el-skeleton-item variant="text"></el-skeleton-item>
        </template>
        <!-- 加载完成后显示的竞价历史表格 -->
        <template #default>
          <el-table :data="bids" style="width: 100%">
            <el-table-column prop="itemName" label="Item Name" width="180"></el-table-column> <!-- 物品名称 -->
            <el-table-column prop="amount" label="Bid Amount" width="180"></el-table-column> <!-- 竞价金额 -->
            <el-table-column prop="status" label="Status"></el-table-column> <!-- 竞价状态 -->
          </el-table>
        </template>
      </el-skeleton>
    </el-card>

    <!-- 编辑资料对话框 -->
    <el-dialog title="Edit Profile" :visible.sync="editProfileDialog">
      <el-form :model="editForm"> <!-- 编辑表单，绑定 editForm -->
        <el-form-item label="Username">
          <el-input v-model="editForm.username"></el-input> <!-- 用户名输入框 -->
        </el-form-item>
        <el-form-item label="Email">
          <el-input v-model="editForm.email"></el-input> <!-- 邮箱输入框 -->
        </el-form-item>
        <el-form-item label="Avatar URL">
          <el-input v-model="editForm.avatar"></el-input> <!-- 头像 URL 输入框 -->
        </el-form-item>
        <!-- 新增用户位置编辑 -->
        <el-form-item label="Location">
          <el-input v-model="editForm.location"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <!-- 取消按钮，关闭对话框 -->
        <el-button @click="editProfileDialog = false">Cancel</el-button>
        <!-- 保存按钮，调用 saveProfile 方法 -->
        <el-button type="primary" @click="saveProfile">Save</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import axios from 'axios'; // 引入 axios 用于发送 HTTP 请求

export default {
  name: 'UserProfile', // 组件名称
  data() {
    return {
      user: {},
      editForm: {},
      bids: [],
      loading: true,
      editProfileDialog: false,
      isAuthenticated: false, // 增加身份验证状态
      isSaving: false, // 增加保存状态标志
    };
  },
  created() {
    const token = localStorage.getItem('authToken');
    if (token) {
      this.isAuthenticated = true;
      this.fetchUserData();
      this.fetchBiddingHistory();
    } else {
      this.$router.push({ name: 'Login', query: { redirect: this.$route.fullPath } });
    }
  },
  methods: {
    // 获取用户数据
    async fetchUserData() {
      const userId = localStorage.getItem('userId');
      try {
        const response = await axios.get(`http://your-api-endpoint/users/${userId}`);
        this.user = response.data;
        this.editForm = { ...response.data };
      } catch (error) {
        console.error('Error fetching user data:', error);
        this.$message.error('Failed to load user data');
      } finally {
        this.loading = false;
      }
    },
    // 获取竞价历史
    async fetchBiddingHistory() {
      const userId = localStorage.getItem('userId');
      try {
        const response = await axios.get(`http://your-api-endpoint/users/${userId}/bids`);
        this.bids = response.data;
      } catch (error) {
        console.error('Error fetching bidding history:', error);
        this.$message.error('Failed to load bidding history');
      } finally {
        this.loading = false;
      }
    },
    // 打开编辑对话框
    openEditDialog() {
      this.editForm = { ...this.user }; // 将当前用户数据复制到编辑表单
      this.editProfileDialog = true; // 设置编辑对话框显示状态为 true
    },
    // 保存编辑后的用户资料
    async saveProfile() {
      if (this.isSaving) return;
      this.isSaving = true;

      const userId = localStorage.getItem('userId');
      try {
        await axios.put(`http://your-api-endpoint/users/${userId}`, this.editForm);
        this.user = { ...this.editForm };
        this.editProfileDialog = false;
        this.$message.success('Profile updated successfully');
      } catch (error) {
        console.error('Error updating profile:', error);
        this.$message.error('Failed to update profile');
      } finally {
        this.isSaving = false;
      }
    },
  },
};
</script>

<style scoped>
.user-profile {
  padding: 20px; /* 内边距 */
  display: flex; /* 使用 flex 布局 */
  flex-direction: column; /* 垂直排列 */
  align-items: center; /* 水平居中 */
}

.profile-header {
  display: flex; /* 使用 flex 布局 */
  align-items: center; /* 垂直居中 */
}

.profile-info {
  margin-left: 20px; /* 左侧间距 */
}

.profile-card,
.bidding-history-card {
  width: 100%; /* 宽度设为 100% */
  max-width: 800px; /* 最大宽度为 800px */
  margin-bottom: 20px; /* 底部间距 */
}

.profile-card h1,
.bidding-history-card h2 {
  margin-bottom: 20px; /* 标题底部间距 */
}
</style>
