<template>
  <div class="item-detail-container">
    <el-row :gutter="20">
      <!-- 左侧：商品图片 -->
      <el-col :span="16" class="image-section">
        <img :src="getBase64Image(item.images)" alt="item.name" class="item-image" />
      </el-col>

      <!-- 右侧：商品信息和出价区域 -->
      <el-col :span="8" class="info-section">
        <h1 class="item-title">{{ item.name }}</h1>
        <p class="item-price">
          Current Bid: US ${{ item.currentBid }} <br />
          Minimum Next Bid: US ${{ minimumNextBid }}
        </p>
        <!-- 如果拍卖已结束，显示提示信息 -->
        <el-alert v-if="isAuctionEnded" title="This auction has ended." type="warning"></el-alert>
        <div class="bid-section">
          <el-input-number v-model="bidAmount" :min="minimumNextBid" :step="bidIncrement" label="Your Bid" />
        </div>
        <div class="button-group">
          <el-button type="primary" @click="bidNow" :disabled="isAuctionEnded" class="bid-now-button">
            Bid Now
          </el-button>
          <el-button type="default" @click="addToCart" class="add-to-cart-button">Add to Cart</el-button>
          <!-- <el-button type="default" @click="addToWishlist" class="add-to-watchlist-button">Add to Watchlist</el-button> -->
        </div>
        <p class="item-description">{{ item.description }}</p>

      </el-col>
    </el-row>

    <!-- 出价历史 -->
    <el-table :data="bids" style="width: 100%" class="bid-history">
      <el-table-column prop="user" label="User" width="150"></el-table-column>
      <el-table-column prop="amount" label="Bid Amount" width="150"></el-table-column>
      <el-table-column prop="time" label="Time"></el-table-column>
    </el-table>

    <!-- 商品详细信息 -->
    <div class="product-details-container">
      <div class="product-details">
        <h2>Product Details</h2>
        <p>{{ item.details }}</p>
      </div>
      <!-- 聊天功能按钮
      <el-button type="primary" @click="goToChat" class="chat-button">Chat with Seller</el-button> -->
    </div>

    <!-- 聊天窗口
    <el-dialog title="Chat with Seller" :visible.sync="chatVisible" width="50%">
      <el-input type="textarea" v-model="chatMessage" placeholder="Type your message here..."
        class="chat-input"></el-input>
      <span slot="footer" class="dialog-footer">
        <el-button @click="chatVisible = false">Cancel</el-button>
        <el-button type="primary" @click="sendMessage">Send</el-button>
      </span>
    </el-dialog> -->

    <div class="recommendations-section">
      <h2>猜你喜欢</h2>
      <el-row :gutter="20">
        <el-col :span="6" v-for="recommendation in recommendedItems" :key="recommendation.id">
          <el-card class="recommendation-item" shadow="hover">
            <img :src="recommendation.imageUrl" alt="recommendation.name" class="recommendation-image" />
            <h3>{{ recommendation.name }}</h3>
            <p>Starting Bid: ${{ recommendation.startingBid }}</p>
            <el-button type="primary" @click="viewItem(recommendation.id)">View Details</el-button>
          </el-card>
        </el-col>
      </el-row>
    </div>

  </div>
</template>

<script>
import axios from 'axios';

const BACKEND_BASE_URL = import.meta.env.VITE_API_BACKEND_BASE_URL;

export default {
  name: 'ItemDetail',
  data() {
    return {
      item: {
        images: [],
        name: '',
        currentBid: 0,
        startingBid: 0, // 确保起拍价从后端读取
        description: '',
        condition: '',
        details: '',
        stock: 1,
        recommendedItems: [],
      },
      quantity: 1,
      bidAmount: 0,
      bidIncrement: 5,
      bids: [],
      // recommendedItems: [],
      // chatVisible: false,
      // chatMessage: '',
      isLoading: true, // 添加加载状态
      error: null, // 添加错误处理
    };
  },
  computed: {
      // 判断拍卖是否已经结束
    isAuctionEnded() {
      const currentTime = new Date();
      return currentTime > new Date(this.item.endTime);
    },
    // 根据当前出价或起拍价计算最小下一次出价
    minimumNextBid() {
      return this.item.currentBid > 0
        ? this.item.currentBid + this.bidIncrement
        : this.item.startingBid;  // 使用起拍价作为初始出价
    },
  },
  mounted() {
    this.checkAuctionStatus();
    setInterval(this.checkAuctionStatus, 600); // 每检查一次
  },
  created() {
    // 页面创建时获取拍卖物品详细信息
    this.fetchItemDetails();
  },
  methods: {
    getBase64Image(image) {
      // 这里假设后端的图片是jpeg格式
      return `data:image/jpeg;base64,${image}`;
    },
    checkAuctionStatus() {
      if (this.isAuctionEnded) {
        this.$message.warning('This auction has ended.');
      }
    },
    // 获取拍卖物品详细信息
    async fetchItemDetails() {
      const itemId = this.$route.params.id;
      this.isLoading = true;
      this.error = null;

      try {
        const response = await axios.get(`${BACKEND_BASE_URL}/auction-items/${itemId}`);
        this.item = response.data;

        console.log('Fetched item details:', this.item.id);  // 确保 item 中有 itemId

        // 设置默认出价为最小下一次出价
        this.bidAmount = this.minimumNextBid;
      } catch (error) {
        console.error('Error fetching item details:', error);
        this.error = 'Failed to fetch item details. Please try again later.';
      } finally {
        this.isLoading = false;
      }
    },
    
    // 获取推荐物品
    async fetchRecommendations() {
      const category = this.item.category;
      this.error = null;

      try {
        const response = await axios.get(`${BACKEND_BASE_URL}/auction-items/recommendations`, {
          params: { category }
        });
        this.recommendedItems = response.data;
      } catch (error) {
        console.error('Error fetching recommendations:', error);
        this.error = 'Failed to fetch recommendations.';
      }
    },

    // 获取出价历史
    async fetchBidHistory() {
      const itemId = this.$route.params.id;
      this.error = null;

      try {
        const response = await axios.get(`${BACKEND_BASE_URL}/auction-items/${itemId}/bids`);
        this.bids = response.data;
      } catch (error) {
        console.error('Error fetching bid history:', error);
        this.error = 'Failed to fetch bid history.';
      }
    },

    // // 出价逻辑
    // bidNow() {
    //   if (this.bidAmount < this.minimumNextBid) {
    //     this.$message.error(`Your bid must be at least US $${this.minimumNextBid}`);
    //     return;
    //   }
    //   // 跳转到支付页面
    //   this.$router.push({
    //     name: 'Payment',
    //     params: {
    //       itemName: this.item.name,
    //       amount: this.bidAmount,
    //       quantity: this.quantity
    //     }
    //   });
    // },
    async bidNow() {
      if (this.bidAmount < this.minimumNextBid) {
        this.$message.error(`Your bid must be at least US $${this.minimumNextBid}`);
        return;
      }

      const bidData = {
        itemId: this.item.id, // 当前物品ID
        userId: localStorage.getItem('userId'), // 数据库中的 USERID，获取用户ID
        bidAmount: this.bidAmount, // 数据库中的 BIDAMOUNT，出价金额
        bidTime: new Date().toISOString(), // 数据库中的 BIDTIME，出价时间
      };

        // 打印 bidData 检查 itemId 是否正确
      console.log('Bid data being sent:', bidData);

      try {
        // 向后端发送POST请求保存出价记录
        await axios.post(`${BACKEND_BASE_URL}/bid`, bidData);
        this.$message.success('Your bid has been placed successfully!');
        // 刷新出价历史以显示最新出价
        this.fetchBidHistory();

      } catch (error) {
        console.error('Error placing bid:', error);
        this.$message.error('Failed to place bid. Please try again.');
      }
    },
  
    // // 添加到购物车
    // addToCart() {
    //   this.$message.success('Added to cart!');
    // },
    // 添加商品到购物车
    async addToCart() {
      const userId = localStorage.getItem('userId');
      try {
        await axios.post(`${BACKEND_BASE_URL}/user/users/${userId}/cart/${this.item.id}`);
        // await axios.delete(`${BACKEND_BASE_URL}/user/users/${userId}/cart/${item.itemId}`);
        this.$message.success('Added to cart!');
      } catch (error) {
        console.error('Error adding item to cart:', error);
        this.$message.error('Failed to add item to cart');
      }
    },
    // // 添加到愿望清单
    // addToWishlist() {
    //   this.$message.success('Added to watchlist!');
    // },

    // // 进入聊天页面
    // goToChat() {
    //   this.$router.push('/chat');
    // },

    // 查看物品详情
    viewItem(id) {
      this.$router.push({ name: 'ItemDetail', params: { id } });
    }
  },
};
</script>

<style scoped>
.item-detail-container {
  display: flex;
  flex-direction: column;
  padding: 40px 50px;
  background-color: #f0f2f5;
  width: 100vw;
  box-sizing: border-box;
}

.image-section {
  display: flex;
  justify-content: center;
  align-items: center;
}

.item-image {
  width: 100%;
  border-radius: 10px;
  object-fit: cover;
}

.info-section {
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.item-title {
  font-size: 1.8rem;
  font-weight: bold;
  margin-bottom: 10px;
}

.item-price {
  font-size: 1.5rem;
  color: #e74c3c;
  margin-bottom: 15px;
}

.item-condition {
  font-size: 1rem;
  margin-bottom: 20px;
}

.item-description {
  font-size: 1rem;
  color: #7f8c8d;
  margin-top: 20px;
}

.quantity-section {
  display: flex;
  align-items: center;
  margin-bottom: 20px;
}

.stock-info {
  margin-left: 10px;
  font-size: 0.9rem;
  color: #7f8c8d;
}

.button-group {
  display: flex;
  flex-direction: column;
  align-items: stretch;
  /* 保证按钮宽度一致 */
  gap: 10px;
}

.bid-now-button {
  width: 100%;
  padding: 15px 20px;
  font-size: 1.2rem;
  background-color: #0066c0;
  color: white;
  border-radius: 5px;
  transition: background-color 0.3s ease;
}

.bid-now-button:hover {
  background-color: #00509d;
}

.add-to-cart-button {
  width: 100%;
  padding: 15px 20px;
  font-size: 1.2rem;
  background-color: white;
  color: #0066c0;
  border: 1px solid #0066c0;
  border-radius: 5px;
  transition: background-color 0.3s ease, color 0.3s ease;
}

.add-to-cart-button:hover {
  background-color: #f5f5f5;
  color: #00509d;
}

.add-to-watchlist-button {
  width: 100%;
  padding: 15px 20px;
  font-size: 1.2rem;
  background-color: white;
  color: #0066c0;
  border: 1px solid #0066c0;
  border-radius: 5px;
  transition: background-color 0.3s ease, color 0.3s ease;
}

.add-to-watchlist-button:hover {
  background-color: #f5f5f5;
  color: #00509d;
}

.bid-history {
  margin-top: 30px;
  border-radius: 10px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.el-button+.el-button {
  margin-left: 0;
}

/* 新增的商品详细信息样式 */
.product-details-container {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-top: 30px;
}

.product-details {
  flex: 1;
  margin-right: 20px;
}

.product-details h2 {
  font-size: 1.5rem;
  margin-bottom: 10px;
}

.product-details p {
  font-size: 1rem;
  color: #7f8c8d;
}

/* 聊天按钮样式
.chat-button {
  flex-shrink: 0;
  padding: 15px 20px;
  font-size: 1.2rem;
  background-color: #ff9900;
  color: white;
  border-radius: 5px;
  transition: background-color 0.3s ease;
}

.chat-button:hover {
  background-color: #e68a00;
} */

/* 新增的“猜你喜欢”样式 */
.recommendations-section {
  margin-top: 40px;
  padding: 20px;
  background-color: #f9f9f9;
  border-radius: 10px;
}

.recommendation-item {
  text-align: center;
  padding: 10px;
}

.recommendation-image {
  width: 100%;
  height: 150px;
  object-fit: cover;
  border-radius: 8px;
  margin-bottom: 10px;
}

.item-image {
  width: 300px; /* 设置图片的固定宽度 */
  height: 300px; /* 设置图片的固定高度 */
  object-fit: cover; /* 保证图片填充整个容器，保持比例且不会变形 */
  border-radius: 10px; /* 可选：给图片加上圆角 */
}
</style>
