# Cryptocurrency Price Change Analysis Based on News

This project aims to create an AI-powered agent that analyzes cryptocurrency price changes over the last 7 days, based on news articles. The goal is to help traders make more informed decisions by understanding the factors driving the price fluctuations of cryptocurrencies.

## **Problem Statement**

Traders need insights into the reasons behind cryptocurrency price changes to determine whether to buy or sell. The task is to develop an AI agent that answers the question:

**"How has the price of Bitcoin (or any other cryptocurrency) changed over the past 7 days? Explain why the price has increased or decreased based on news articles."**

The agent should be able to:
- Fetch cryptocurrency prices and relevant news articles for the past week.
- Use AI to interpret the news and correlate it with price changes.

## **Features**

- **Price Analysis**: The AI agent tracks and analyzes the 7-day price change of Bitcoin and other cryptocurrencies.
- **News Interpretation**: It fetches news articles related to the selected cryptocurrency and interprets the reasons behind price fluctuations.
- **Interactive Bot**: *(For advanced users)* A Telegram bot that interacts with users, providing real-time price change analysis and explanations based on the latest news.

## **Technologies Used**

- **Semantic Kernel**: A framework for integrating AI models into various applications.
- **GPT-4**: For generating natural language explanations of the price changes based on news.
- **Cryptocurrency APIs**: For fetching cryptocurrency prices and news, including services like Cryptopanic, NewsAPI, CoinMarketCap, CoinGecko, etc.
- **Telegram Bot** *(Optional for advanced users)*: A bot that provides users with real-time cryptocurrency price insights.

## **Setup**

To set up the project, follow these steps:

1. **Clone the repository**:

   ```bash
   git clone https://github.com/yourusername/crypto-price-analysis.git
   cd crypto-price-analysis
Install dependencies. You can use NuGet to install required libraries, or if you're using .NET, you can run:

bash
dotnet restore
Configure your API keys for cryptocurrency price services and news APIs:

You can get an API key from services like Cryptopanic, CoinGecko, or CoinMarketCap.
Set the API keys in the appsettings.json or environment variables.
Run the application:

bash
dotnet run
If you're using the Telegram bot, make sure to add your bot's token in the appropriate configuration.

Advanced Features
For advanced users, a Telegram bot has been implemented that incorporates all of the above functionality. It fetches the latest cryptocurrency prices, interprets the news, and explains the price changes. You can interact with it at:

Telegram Bot - Crypto Price Analysis

If you've developed your own Telegram bot with similar functionality, feel free to share it in the Discord community.

Acknowledgements
Cryptopanic: For providing news aggregation on cryptocurrencies.
NewsAPI: For fetching news articles related to cryptocurrencies.
CoinMarketCap, CoinGecko, and others: For providing cryptocurrency market data.
License
This project is licensed under the MIT License - see the LICENSE file for details.

Contact
For any questions or contributions, feel free to open an issue or contact the maintainer at [your email address].

markdown

### Key Formatting Breakdown:
- **Bold Text**: For section titles like **Problem Statement**, **Features**, **Technologies Used**, etc.
- **Code Block**: For commands like cloning the repository, installing dependencies, and running the app, enclosed in triple backticks (\`\`\`).
- **Links**: For the Telegram bot and Discord community.
- **Headings**: Used to structure the document (e.g., `#` for main headings, `##` for subheadings).

This structure will look clean and formatted properly when viewed on GitHub.
