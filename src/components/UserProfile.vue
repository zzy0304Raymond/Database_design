<template>
  <div class="user-profile">
    <el-card class="profile-card">
      <div class="profile-header">
        <el-avatar :src="user.avatar" size="large"></el-avatar>
        <div class="profile-info">
          <h1>User Profile</h1>
          <el-button type="primary" @click="openEditDialog">Edit Profile</el-button>
        </div>
      </div>
      <div class="user-info">
        <el-skeleton :loading="loading" animated>
          <template #template>
            <el-skeleton-item variant="text"></el-skeleton-item>
            <el-skeleton-item variant="text"></el-skeleton-item>
          </template>
          <template #default>
            <p>Username: {{ user.username }}</p>
            <p>Email: {{ user.email }}</p>
          </template>
        </el-skeleton>
      </div>
    </el-card>
    <el-card class="bidding-history-card">
      <h2>Bidding History</h2>
      <el-skeleton :loading="loading" animated count="5">
        <template #template>
          <el-skeleton-item variant="text"></el-skeleton-item>
        </template>
        <template #default>
          <el-table :data="bids" style="width: 100%">
            <el-table-column prop="itemName" label="Item Name" width="180"></el-table-column>
            <el-table-column prop="amount" label="Bid Amount" width="180"></el-table-column>
            <el-table-column prop="status" label="Status"></el-table-column>
          </el-table>
        </template>
      </el-skeleton>
    </el-card>

    <el-dialog title="Edit Profile" :visible.sync="editProfileDialog">
      <el-form :model="editForm">
        <el-form-item label="Username">
          <el-input v-model="editForm.username"></el-input>
        </el-form-item>
        <el-form-item label="Email">
          <el-input v-model="editForm.email"></el-input>
        </el-form-item>
        <el-form-item label="Avatar URL">
          <el-input v-model="editForm.avatar"></el-input>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="editProfileDialog = false">Cancel</el-button>
        <el-button type="primary" @click="saveProfile">Save</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'UserProfile',
  data() {
    return {
      user: {
        username: 'JohnDoe',
        email: 'john@example.com',
        avatar: 'https://via.placeholder.com/100',
      },
      editForm: {
        username: '',
        email: '',
        avatar: '',
      },
      bids: [],
      loading: true,
      editProfileDialog: false,
    };
  },
  created() {
    this.fetchUserData();
    this.fetchBiddingHistory();
  },
  methods: {
    fetchUserData() {
      const userId = localStorage.getItem('userId');
      axios.get(`http://your-api-endpoint/users/${userId}`)
        .then(response => {
          this.user = response.data;
          this.editForm = { ...response.data }; // 初始化编辑表单
          this.loading = false;
        })
        .catch(error => {
          console.error('Error fetching user data:', error);
          this.loading = false;
        });
    },
    fetchBiddingHistory() {
      const userId = localStorage.getItem('userId');
      axios.get(`http://your-api-endpoint/users/${userId}/bids`)
        .then(response => {
          this.bids = response.data;
          this.loading = false;
        })
        .catch(error => {
          console.error('Error fetching bidding history:', error);
          this.loading = false;
        });
    },
    openEditDialog() {
      this.editForm = { ...this.user }; // 将当前用户数据复制到编辑表单
      this.editProfileDialog = true;
    },
    saveProfile() {
      const userId = localStorage.getItem('userId');
      axios.put(`http://your-api-endpoint/users/${userId}`, this.editForm)
        .then(response => {
          this.user = { ...this.editForm };
          this.editProfileDialog = false;
          this.$message({
            message: 'Profile updated successfully',
            type: 'success'
          });
        })
        .catch(error => {
          console.error('Error updating profile:', error);
          this.$message.error('Failed to update profile');
        });
    },
  },
};
</script>

<style scoped>
.user-profile {
  padding: 20px;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.profile-header {
  display: flex;
  align-items: center;
}

.profile-info {
  margin-left: 20px;
}

.profile-card,
.bidding-history-card {
  width: 100%;
  max-width: 800px;
  margin-bottom: 20px;
}

.profile-card h1,
.bidding-history-card h2 {
  margin-bottom: 20px;
}
</style>
