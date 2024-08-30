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
            <el-list-item v-for="message in paginatedMessages" :key="message.id">
              <el-card shadow="hover" class="message-card">
                <h3>{{ message.title }}</h3>
                <p>{{ message.content }}</p>
                <small>{{ formatTimestamp(message.timestamp) }}</small>
                <el-button type="danger" size="mini" @click="deleteMessage(message.id)">Delete</el-button>
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
    handlePageChange(page) {
      this.currentPage = page;
    },
    deleteMessage(messageId) {
      const userId = localStorage.getItem('userId');
      axios.delete(`http://your-api-endpoint/users/${userId}/messages/${messageId}`)
        .then(response => {
          this.messages = this.messages.filter(message => message.id !== messageId);
          this.$message({
            message: 'Message deleted successfully',
            type: 'success',
          });
        })
        .catch(error => {
          console.error('Error deleting message:', error);
          this.$message.error('Failed to delete message');
        });
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
}
</style>
