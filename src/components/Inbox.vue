<template>
  <div class="inbox">
    <el-card class="inbox-card">
      <h1>Inbox</h1>
      <el-skeleton :loading="loading" animated>
        <template #template>
          <el-skeleton-item variant="text"></el-skeleton-item>
          <el-skeleton-item variant="text"></el-skeleton-item>
          <el-skeleton-item variant="text"></el-skeleton-item>
        </template>
        <template #default>
          <el-empty v-if="messages.length === 0" description="No messages found">
            <el-button type="primary" @click="fetchMessages">Reload</el-button>
          </el-empty>
          <el-list v-else>
            <el-list-item v-for="message in paginatedMessages" :key="message.id" @click="openChat(message.senderId)">
              <el-card shadow="hover" class="message-card">
                <div class="message-header">
                  <img :src="message.senderAvatar" alt="avatar" class="avatar">
                  <div class="message-info">
                    <h3>{{ message.senderName }}</h3>
                    <p>{{ truncateMessage(message.content) }}</p>
                  </div>
                </div>
                <small>{{ formatTimestamp(message.timestamp) }}</small>
              </el-card>
            </el-list-item>
          </el-list>
          <el-pagination
            v-if="totalPages > 1"
            background
            layout="prev, pager, next"
            :current-page.sync="currentPage"
            :page-size="itemsPerPage"
            :total="messages.length"
            @current-change="handlePageChange"
          ></el-pagination>
        </template>
      </el-skeleton>
    </el-card>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'Inbox',
  data() {
    return {
      messages: [],
      loading: true,
      currentPage: 1,
      itemsPerPage: 5,
    };
  },
  computed: {
    paginatedMessages() {
      const start = (this.currentPage - 1) * this.itemsPerPage;
      const end = start + this.itemsPerPage;
      return this.messages.slice(start, end);
    },
    totalPages() {
      return Math.ceil(this.messages.length / this.itemsPerPage);
    },
  },
  created() {
    this.fetchMessages();
  },
  methods: {
    fetchMessages() {
      this.loading = true;
      const userId = localStorage.getItem('userId');
      axios.get(`http://your-api-endpoint/users/${userId}/messages`)
        .then(response => {
          this.messages = response.data;
          this.loading = false;
        })
        .catch(error => {
          console.error('Error fetching messages:', error);
          this.loading = false;
        });
    },
    formatTimestamp(timestamp) {
      const date = new Date(timestamp);
      const year = date.getFullYear();
      const month = String(date.getMonth() + 1).padStart(2, '0');
      const day = String(date.getDate()).padStart(2, '0');
      const hours = String(date.getHours()).padStart(2, '0');
      const minutes = String(date.getMinutes()).padStart(2, '0');
      return `${year}-${month}-${day} ${hours}:${minutes}`;
    },
    truncateMessage(message) {
      return message.length > 50 ? message.slice(0, 50) + '...' : message;
    },
    handlePageChange(page) {
      this.currentPage = page;
    },
    openChat(senderId) {
      this.$router.push({ name: 'Chat', params: { senderId } });
    },
  },
};
</script>

<style scoped>
.inbox {
  padding: 20px;
  display: flex;
  justify-content: center;
  align-items: center;
  flex-direction: column;
}

.inbox-card {
  width: 100%;
  max-width: 800px;
}

.message-card {
  margin-bottom: 10px;
  cursor: pointer;
  transition: background-color 0.3s;
}

.message-card:hover {
  background-color: #f5f5f5;
}

.message-header {
  display: flex;
  align-items: center;
}

.avatar {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  margin-right: 15px;
}

.message-info {
  flex: 1;
}

.message-info h3 {
  margin: 0;
  font-size: 1rem;
}

.message-info p {
  margin: 0;
  color: #555;
}
</style>
