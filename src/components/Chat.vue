<template>
    <div class="chat-container">
      <div class="chat-window">
        <div class="chat-header">
          <h2>Chat with {{ sellerName }}</h2>
        </div>
        <div class="chat-body" ref="chatBody">
          <div v-for="message in messages" :key="message.id" class="chat-message" :class="{'my-message': message.from === currentUser}">
            <div class="message-info">
              <span class="message-sender">{{ message.from === currentUser ? 'You' : sellerName }}</span>
              <span class="message-time">{{ formatTime(message.time) }}</span>
            </div>
            <p>{{ message.text }}</p>
          </div>
          <div v-if="isTyping" class="typing-indicator">
            {{ sellerName }} is typing...
          </div>
        </div>
        <div class="chat-input-area">
          <el-input
            type="textarea"
            v-model="chatMessage"
            placeholder="Type your message here..."
            class="chat-input"
            @keyup.enter="sendMessage"
            @input="notifyTyping"
          ></el-input>
          <el-button type="primary" @click="sendMessage">Send</el-button>
        </div>
      </div>
    </div>
  </template>
  
  <script>
  import io from 'socket.io-client';
  
  export default {
    name: 'Chat',
    data() {
      return {
        chatMessage: '',
        messages: [],
        socket: null,
        currentUser: 'buyer1',
        sellerName: 'Seller',
        isTyping: false,
        typingTimeout: null, // 用于打字通知的计时器
      };
    },
    created() {
      const senderId = this.$route.params.senderId; // 获取路由参数中的 senderId
      this.fetchChatHistory(senderId); // 获取与该用户的聊天历史记录
      this.initializeSocket(senderId); // 初始化 WebSocket 连接
    },
    methods: {
      async fetchChatHistory(senderId) {
        try {
          const response = await axios.get(`http://your-api-endpoint/chats/${senderId}`);
          this.messages = response.data;
          this.scrollToBottom();
        } catch (error) {
          console.error('Error fetching chat history:', error);
          this.$message.error('Failed to load chat history.');
        }
      },
      initializeSocket(senderId) {
        this.socket = io('http://localhost:3000');
        this.socket.emit('joinRoom', senderId);
  
        this.socket.on('receiveMessage', (message) => {
          this.messages.push(message);
          this.isTyping = false;
          this.scrollToBottom();
        });
  
        this.socket.on('typing', () => {
          this.isTyping = true;
        });
  
        this.socket.on('stopTyping', () => {
          this.isTyping = false;
        });
      },
      sendMessage() {
        if (this.chatMessage.trim() !== '') {
          const message = {
            from: this.currentUser,
            text: this.chatMessage,
            time: new Date(),
          };
          this.socket.emit('sendMessage', message);
          this.socket.emit('stopTyping');
          this.messages.push(message);
          this.chatMessage = '';
          this.scrollToBottom();
        }
      },
      scrollToBottom() {
        this.$nextTick(() => {
          const chatBody = this.$refs.chatBody;
          chatBody.scrollTop = chatBody.scrollHeight;
        });
      },
      formatTime(date) {
        return new Date(date).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
      },
      notifyTyping() {
        if (this.typingTimeout) clearTimeout(this.typingTimeout);
        this.socket.emit('typing');
        this.typingTimeout = setTimeout(() => {
          this.socket.emit('stopTyping');
        }, 500);
      },
    },
    beforeDestroy() {
      if (this.socket) {
        this.socket.disconnect();
      }
    }
  };
  </script>
  
  <style scoped>
  .chat-container {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    background: linear-gradient(to right, #6dd5fa, #2980b9);
  }
  
  .chat-window {
    width: 100%;
    max-width: 500px;
    height: 80vh;
    display: flex;
    flex-direction: column;
    border: 1px solid #ccc;
    border-radius: 15px;
    overflow: hidden;
    background-color: #f0f4f8; /* 聊天框背景颜色变浅 */
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
  }
  
  .chat-message {
    margin-bottom: 15px;
    padding: 10px;
    border-radius: 10px;
    max-width: 80%;
    background-color: #e1f5fe; /* 调整聊天气泡背景颜色为更浅的蓝色 */
    align-self: flex-start;
  }
  
  .my-message {
    background-color: #bbdefb; /* 调整自己消息的气泡背景颜色为更浅的蓝色 */
    color: #0d47a1; /* 文本颜色 */
    align-self: flex-end;
  }
  
  .chat-header {
    background-color: #64b5f6; /* 聊天标题栏背景颜色变浅 */
    color: white;
    padding: 15px;
    text-align: center;
    font-size: 1.5rem;
    border-bottom: 1px solid #ddd;
  }
  
  .chat-body {
    flex: 1;
    padding: 15px;
    overflow-y: auto;
    background-color: #f4f7f9;
  }
  
  .message-info {
    display: flex;
    justify-content: space-between;
    margin-bottom: 5px;
    font-size: 0.8rem;
    color: #555;
  }
  
  .typing-indicator {
    font-size: 0.9rem;
    color: #666;
    font-style: italic;
    margin: 10px 0;
  }
  
  .chat-input-area {
    display: flex;
    padding: 15px;
    background-color: #fff;
    border-top: 1px solid #ddd;
  }
  
  .chat-input {
    flex: 1;
    margin-right: 10px;
  }
  
  .chat-input ::-webkit-scrollbar {
    width: 6px;
  }
  
  .chat-input ::-webkit-scrollbar-thumb {
    background-color: #ccc;
    border-radius: 10px;
  }
  </style>
  