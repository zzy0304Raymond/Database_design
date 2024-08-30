<template>
  <div class="item-detail-container">
    <!-- 商品详情卡片 -->
    <el-card class="detail-card" shadow="always">
      <el-row :gutter="20">
        <!-- 左侧：商品图片 -->
        <el-col :span="10" class="image-section">
          <img :src="item.imageUrl" alt="item.name" class="item-image" />
        </el-col>

        <!-- 右侧：商品信息和出价区域 -->
        <el-col :span="14" class="info-section">
          <div class="info-wrapper">
            <h1 class="item-title">{{ item.name }}</h1>
            <p class="item-price">Starting Bid: ${{ item.startingBid }}</p>
            <p class="item-current-bid">Current Bid: ${{ item.currentBid || item.startingBid }}</p>
            <p class="item-description">{{ item.description }}</p>

            <!-- 竞价输入框和按钮 -->
            <div class="bid-section">
              <el-input
                v-model="bidAmount"
                placeholder="Enter your bid"
                type="number"
                class="bid-input"
              />
              <el-button type="primary" @click="placeBid" class="bid-button">
                Place a Bid
              </el-button>
            </div>
          </div>
        </el-col>
      </el-row>

      <!-- 出价历史 -->
      <el-table :data="bids" style="width: 100%" class="bid-history">
        <el-table-column prop="user" label="User" width="150"></el-table-column>
        <el-table-column prop="amount" label="Bid Amount" width="150"></el-table-column>
        <el-table-column prop="time" label="Time"></el-table-column>
      </el-table>
    </el-card>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'ItemDetail',
  data() {
    return {
      item: {},  // 商品详情对象
      bidAmount: 0,  // 用户输入的竞价金额
      bids: [],  // 出价历史记录
    };
  },
  created() {
    this.fetchItemDetails();  // 组件创建时获取商品详情
    this.fetchBidHistory();  // 获取出价历史
  },
  methods: {
    // 获取商品详情的方法
    fetchItemDetails() {
      const itemId = this.$route.params.id;  // 从路由参数中获取商品ID
      axios.get(`http://your-api-endpoint/auction-items/${itemId}`)
        .then(response => {
          this.item = response.data;  // 将响应数据赋值给 item 对象
        })
        .catch(error => {
          console.error('Error fetching item details:', error);  // 处理错误
        });
    },
    // 获取出价历史
    fetchBidHistory() {
      const itemId = this.$route.params.id;
      axios.get(`http://your-api-endpoint/auction-items/${itemId}/bids`)
        .then(response => {
          this.bids = response.data;
        })
        .catch(error => {
          console.error('Error fetching bid history:', error);
        });
    },
    // 提交竞价的方法
    placeBid() {
      const userId = localStorage.getItem('userId');  // 获取用户ID
      axios.post(`http://your-api-endpoint/auction-items/${this.item.id}/bids`, {
        userId: userId,
        amount: this.bidAmount,  // 提交的竞价金额
      })
      .then(response => {
        this.$message.success('Your bid has been placed successfully!');  // 显示成功提示
        this.fetchBidHistory(); // 更新出价历史
      })
      .catch(error => {
        this.$message.error('Failed to place bid. Please try again.');  // 显示错误提示
      });
    }
  },
};
</script>

<style scoped>
.item-detail-container {
  display: flex;
  justify-content: center; /* 中心对齐 */
  padding: 40px 20px; /* 页面内边距 */
  background-color: #f0f2f5; /* 背景颜色 */
}

.detail-card {
  max-width: 1000px; /* 卡片最大宽度 */
  width: 100%; /* 卡片宽度自适应 */
  padding: 20px; /* 卡片内边距 */
  border-radius: 10px; /* 圆角 */
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1); /* 阴影效果 */
  background-color: #ffffff; /* 卡片背景色 */
}

.image-section {
  display: flex; 
  justify-content: center; /* 图片居中对齐 */
  align-items: center;
}

.item-image {
  max-width: 100%; /* 图片最大宽度 */
  border-radius: 10px; /* 圆角 */
  margin-bottom: 20px; /* 图片与下方内容间距 */
  object-fit: cover; /* 图片按比例缩放 */
}

.info-section {
  display: flex;
  flex-direction: column; /* 垂直排列 */
  justify-content: center;
}

.info-wrapper {
  padding: 10px 20px; /* 内边距 */
}

.item-title {
  font-size: 2.2rem; /* 标题字体大小 */
  color: #2c3e50; /* 标题颜色 */
  margin-bottom: 10px; /* 标题底部间距 */
  font-weight: bold; /* 加粗 */
}

.item-price,
.item-current-bid {
  font-size: 1.5rem; /* 价格字体大小 */
  color: #e74c3c; /* 价格颜色 */
  margin-bottom: 10px; /* 与描述的间距 */
}

.item-description {
  font-size: 1.1rem; /* 描述字体大小 */
  color: #7f8c8d; /* 描述颜色 */
  margin-bottom: 20px; /* 描述与竞价区的间距 */
}

.bid-section {
  display: flex; /* 使用 Flex 布局 */
  gap: 10px; /* 输入框和按钮之间的间距 */
  margin-top: 20px; /* 与上方内容间距 */
}

.bid-input {
  width: 180px; /* 输入框宽度 */
}

.bid-button {
  padding: 10px 20px; /* 按钮内边距 */
  transition: background-color 0.3s ease; /* 背景色过渡效果 */
}

.bid-button:hover {
  background-color: #3498db; /* 按钮悬停时的背景颜色 */
}

.bid-history {
  margin-top: 30px; /* 出价历史与其他内容的间距 */
  border-radius: 10px; /* 表格圆角 */
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1); /* 表格阴影 */
}
</style>
