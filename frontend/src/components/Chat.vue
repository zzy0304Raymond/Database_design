<template>
    <div class="chat-container">
      <div class="chat-messages">
        <div v-for="message in chatMessages" :key="message.chattime" class="message">
          <p>{{ message.content }}</p>
        </div>
      </div>
      <div class="chat-input">
        <input v-model="newMessage" placeholder="输入消息..." />
        <button @click="sendMessage">发送</button>
      </div>
    </div>
  </template>
  
  <script>
  import axios from 'axios';
  const BACKEND_BASE_URL = import.meta.env.VITE_API_BACKEND_BASE_URL;
  
  export default {
    data() {
      return {
        chatMessages: [],  // 聊天记录
        newMessage: '',    // 新消息内容
        userid: 'mock_user_123',   // 模拟的用户id，假设没有登录功能
      };
    },
    mounted() {
      this.getChatMessages(10);  // 默认获取最近10条消息
    },
    methods: {
      // 获取聊天记录
      async getChatMessages(n) {
        try {
          const response = await axios.get(`${BACKEND_BASE_URL}/chat?n=${n}`);
          console.log('获取的聊天记录:', response.data);  // 调试用，输出聊天记录
          this.chatMessages = response.data;  // 假设返回的数据是数组格式 [{content, chattime}]
        } catch (error) {
          console.error('获取聊天记录失败', error);
        }
      },
      // 发送消息
      async sendMessage() {
        if (this.newMessage.trim() === '') return;  // 确保消息不为空
        
        const messageData = {
          userId: localStorage.getItem('userId'),
          chattime: new Date().toISOString(),  // 发送的时间
          content: this.newMessage,  // 发送的消息内容
        };
  
        try {
            const response = await axios.post(`${BACKEND_BASE_URL}/chat`, messageData);
            console.log('发送消息成功:', response.data);  // 调试用，输出发送消息的返回结果
            
            // 手动添加新消息到chatMessages数组，避免等待网络延迟
            this.chatMessages.push(messageData);

            // 不再调用getChatMessages(10)，减少不必要的API调用
        } catch (error) {
            console.error('发送消息失败', error);
        }
      },
    },
  };
  </script>
  
  <style scoped>
  .chat-container {
    display: flex;
    flex-direction: column;
    height: 100vh;
    justify-content: space-between;
  }
  
  .chat-messages {
    flex-grow: 1;
    overflow-y: auto;
    padding: 10px;
    border: 1px solid #ccc;
  }
  
  .message {
    margin-bottom: 10px;
  }
  
  .chat-input {
    display: flex;
    padding: 10px;
  }
  
  input {
    flex-grow: 1;
    padding: 5px;
    border: 1px solid #ccc;
    border-radius: 4px;
  }
  
  button {
    margin-left: 10px;
    padding: 5px 10px;
    border: none;
    background-color: #28a745;
    color: white;
    border-radius: 4px;
    cursor: pointer;
  }
  
  button:hover {
    background-color: #218838;
  }
  </style>
  