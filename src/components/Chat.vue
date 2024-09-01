<template>
    <div class="chat-container">
      <h1>Chat with Seller</h1>
      <el-input
        type="textarea"
        v-model="chatMessage"
        placeholder="Type your message here..."
        class="chat-input"
      ></el-input>
      <div class="chat-actions">
        <el-button type="default" @click="cancelChat">Cancel</el-button>
        <el-button type="primary" @click="sendMessage">Send</el-button>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: 'Chat',
    data() {
      return {
        chatMessage: '',  // 聊天内容
      };
    },
    methods: {
      cancelChat() {
        this.$router.push(-1);  // 返回上一页
      },
      sendMessage() {
        console.log("Message sent:", this.chatMessage);
        this.$message.success('Message sent!');
        this.chatMessage = '';  // 清空输入框
      }
    }
  };
  </script>
  
  <style scoped>
  .chat-container {
    padding: 40px;
    background-color: #f0f2f5;
    max-width: 800px;
    margin: 0 auto;
    border-radius: 10px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  }
  
  .chat-input {
    width: 100%;
    margin-bottom: 20px;
  }
  
  .chat-actions {
    display: flex;
    justify-content: flex-end;
    gap: 10px;
  }
  </style>
  