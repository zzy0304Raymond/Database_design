<template>
  <div class="user-profile" v-if="isAuthenticated"> 
    <!-- 用户资料卡片 -->
    <el-card class="profile-card">
      <div class="profile-header">
        <div class="profile-info">
          <h1>User Profile</h1>
          <!-- 编辑资料按钮，点击时打开编辑模式 -->
          <el-button type="primary" @click="isEditing = !isEditing"> <!-- 切换编辑模式 -->
            {{ isEditing ? 'Cancel' : 'Edit Profile' }} <!-- 根据编辑状态改变按钮文字 -->
          </el-button>
        </div>
      </div>
      <div class="user-info">
        <!-- 骨架屏组件，加载时显示占位符 -->
        <el-skeleton :loading="loading" animated>
          <template #template>
            <el-skeleton-item variant="text"></el-skeleton-item>
            <el-skeleton-item variant="text"></el-skeleton-item>
          </template>
          <template #default>
            <!-- 根据编辑状态显示不同内容 -->
            <div v-if="!isEditing">
              <p>Username: {{ user.username }}</p>
              <p>Email: {{ user.email }}</p>
            </div>
            <el-form v-else :model="editForm"> <!-- 编辑表单 -->
              <el-form-item label="Username">
                <el-input v-model="editForm.username"></el-input> 
              </el-form-item>
              <el-form-item label="Email">
                <el-input v-model="editForm.email"></el-input> 
              </el-form-item>
              <el-button type="primary" @click="saveProfile">Save</el-button> <!-- 保存按钮 -->
            </el-form>
          </template>
        </el-skeleton>
      </div>
    </el-card>

    <!-- 竞价历史卡片 -->
    <el-card class="bidding-history-card">
      <h2>Bidding History</h2>
      <el-skeleton :loading="loading" animated :count="5">
        <template #template>
          <el-skeleton-item variant="text"></el-skeleton-item>
        </template>
        <template #default>
          <el-table :data="bids" style="width: 100%">
            <el-table-column prop="itemName" label="Item Name" width="180"></el-table-column> 
            <el-table-column prop="amount" label="Your Bid" width="180"></el-table-column> 
            <el-table-column prop="currentPrice" label="Current Price" width="180"></el-table-column>
            <el-table-column prop="endtime" label="End Time"></el-table-column>
            <el-table-column label="Action" width="150">
              <template #default="scope">
                <!-- 判断Endtime是否已到，决定显示内容 -->
                <div v-if="new Date() >= new Date(scope.row.endtime)">
                  <el-button v-if="scope.row.status === 'Pending Payment'" 
                             type="primary" 
                             size="small" 
                             @click="goToPayment(scope.row)">
                    Pay Now
                  </el-button>
                </div>
                <div v-else>
                  <p>Your Bid: {{ scope.row.amount }}</p>
                  <p>Current Price: {{ scope.row.currentPrice }}</p>
                  <p>Check if you need to increase your bid.</p>
                </div>
              </template>
            </el-table-column>
          </el-table>
        </template>
      </el-skeleton>
    </el-card>

    <!-- 购物车卡片 -->
    <el-card class="cart-card">
      <h2>Your Cart</h2>
      <el-skeleton :loading="loading" animated>
        <template #default>
          <el-table :data="cartItems" style="width: 100%">
            <el-table-column prop="itemName" label="Item Name" width="180">
              <template #default="scope">
                <el-link @click="goToItemDetail(scope.row.itemId)">
                  {{ scope.row.itemName }}
                </el-link>
              </template>
            </el-table-column>
            <el-table-column prop="price" label="Price" width="100"></el-table-column>
            <el-table-column label="Action" width="150">
              <template #default="scope">
                <el-button @click="removeFromCart(scope.row)" type="danger" size="small">Remove</el-button>
              </template>
            </el-table-column>
          </el-table>
        </template>
      </el-skeleton>
    </el-card>

    <el-card class="pending-payment-card">
      <h2>Pending Payments</h2>
      <el-skeleton :loading="loading" animated>
        <template #default>
          <el-table :data="pendingPayments" style="width: 100%">
            <el-table-column prop="itemName" label="Item Name" width="180"></el-table-column>
            <el-table-column prop="price" label="Price" width="100"></el-table-column>
            <el-table-column label="Action" width="150">
              <template #default="scope">
                <el-button @click="goToPayment(scope.row)" type="primary" size="small">Pay Now</el-button>
              </template>
            </el-table-column>
          </el-table>
        </template>
      </el-skeleton>
    </el-card>

    <el-button type="danger" @click="logout">Logout</el-button>
  </div>
</template>

<script>
import axios from 'axios';

const BACKEND_BASE_URL = import.meta.env.VITE_API_BACKEND_BASE_URL;

export default {
  name: 'UserProfile',
  data() {
    return {
      user: {},
      editForm: {},
      bids: [],
      cartItems: [], // 新增购物车数据
      loading: true,
      isAuthenticated: false,
      isSaving: false,
      isEditing: false, // 增加编辑模式状态
    };
  },
  created() {
    const token = localStorage.getItem('authToken');
    if (token) {
      this.isAuthenticated = true;
      this.user = {};  // 清除上次的用户数据
      this.fetchUserData();
      this.fetchBiddingHistory();
      this.fetchCartItems();
      this.fetchPendingPayments(); // 获取待支付商品
    } else {
      this.$router.push({ name: 'Login', query: { redirect: this.$route.fullPath } });
    }
  },
  methods: {
    // 获取用户数据
    async fetchUserData() {
      const userId = localStorage.getItem('userId');
      try {
        const response = await axios.get(`${BACKEND_BASE_URL}/user/users/${userId}`);
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
        const response = await axios.get(`${BACKEND_BASE_URL}/user/users/${userId}/bids`);
        this.bids = response.data;
      } catch (error) {
        console.error('Error fetching bidding history:', error);
        this.$message.error('Failed to load bidding history');
      } finally {
        this.loading = false;
      }
    },
    // 保存编辑后的用户资料
    async saveProfile() {
      if (this.isSaving) return;
      this.isSaving = true;

      const userId = localStorage.getItem('userId');
      try {
        await axios.put(`${BACKEND_BASE_URL}/user/users/${userId}`, this.editForm);
        this.user = { ...this.editForm };
        this.isEditing = false; // 保存成功后退出编辑模式
        this.$message.success('Profile updated successfully');
      } catch (error) {
        console.error('Error updating profile:', error);
        this.$message.error('Failed to update profile');
      } finally {
        this.isSaving = false;
      }
    },
    async fetchCartItems() {
      const userId = localStorage.getItem('userId');
      try {
        const response = await axios.get(`${BACKEND_BASE_URL}/user/users/${userId}/cart`);
        // 将返回的数据简化为只保留 itemName、price 和 itemId
        this.cartItems = response.data.map(item => ({
          itemName: item.itemName,
          price: item.price,
          itemId: item.itemId
        }));
      } catch (error) {
        console.error('Error fetching cart items:', error);
        this.$message.error('Failed to load cart items');
      }
    },
    
    goToItemDetail(itemId) {
      this.$router.push({ name: 'ItemDetail', params: { id: itemId } });
    },
    // 从购物车中移除物品
    async removeFromCart(item) {
      const userId = localStorage.getItem('userId');
      try {
        await axios.delete(`${BACKEND_BASE_URL}/user/users/${userId}/cart/${item.itemId}`);
        this.$message.success('Item removed from cart');
        this.fetchCartItems();
      } catch (error) {
        console.error('Error removing item from cart:', error);
        this.$message.error('Failed to remove item from cart');
      }
    },
    logout() {
      // 清除localStorage中的用户相关数据
      localStorage.removeItem('authToken');
      localStorage.removeItem('userId');
      this.isAuthenticated = false; // 更新身份验证状态

      // 重定向到登录页面
      this.$router.push({ name: 'Login' });
    },

    async fetchPendingPayments() {
      const userId = localStorage.getItem('userId');
      try {
        const response = await axios.get(`${BACKEND_BASE_URL}/user/users/${userId}/pending-payments`);
        this.pendingPayments = response.data.map(item => ({
          itemName: item.itemName,
          price: item.price,
          orderId: item.orderId,
        }));
      } catch (error) {
        console.error('Error fetching pending payments:', error);
        this.$message.error('Failed to load pending payments');
      }
    },

    goToPayment(order) {
      this.$router.push({ name: 'Payment', params: { orderId: order.orderId } });
    },
    
  },
};
</script>


<style scoped>
.profile-header {
  display: flex;
  /* 使用 flex 布局 */
  align-items: center;
  /* 垂直居中 */
}

.profile-info {
  margin-left: 20px;
  /* 左侧间距 */
}

.user-profile {
  padding: 20px;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.profile-card,
.bidding-history-card,
.cart-card,
.pending-payment-card {
  width: 100%;
  max-width: 800px; /* 设置所有card的最大宽度相同 */
  margin-bottom: 20px; /* 保持卡片之间的间距 */
}

.profile-card h1,
.bidding-history-card h2,
.cart-card h2,
.pending-payment-card h2 {
  margin-bottom: 20px;
}
</style>
