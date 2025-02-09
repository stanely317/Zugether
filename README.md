<!DOCTYPE html>
<html lang="zh-Hant">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
</head>

<body>
    <h1>Zugether合租網站</h1>
    <h2>目錄：</h2>
    <ul>
        <li><a href="#description">專案說明</a></li>
        <li><a href="#myJob">我負責的項目</a></li>
        <li><a href="#technologies">技術應用</a></li>
        <li><a href="#database">資料庫設計</a></li>
        <li><a href="#feature">功能說明</a></li>
    </ul>
    <h2 id="description">專案說明：</h2>
    <ul>
        <li>
            對於在外生活的學生與上班族，龐大支出來源之一就是房租，
            若能有合適的室友來分擔，不僅能緩解經濟壓力，還能結交新朋友。
            因此，我們提供 Zugether 合租網站，標準化房屋資訊、提供篩選器，
            幫助您快速找到理想合租夥伴。
        </li>
    </ul>
    <h2 id="myJob">我負責的項目：</h2>
    <ul>
        <li>資料庫設計：依功能需求設計資料表、欄位與Table間的關聯</li>
        <li>資料庫上雲：本地建置完成後備份上傳至Cloud SQL</li>
        <li>刊登/刪除房間：透過Transaction確保資料更新時的正確性</li>      
        <li>修改房間：前端確認資料異動後才送出請求，減輕後端負擔</li>        
    </ul>
    <h2 id="technologies">使用技術：</h2>
    <ul>
        <li>前端：</li>
        <ul>
            <li>HTML、CSS</li>
            <li>JavaScript & jQuery</li>
            <li>Bootstrap</li>
        </ul>
        <li>後端：</li>
        <ul>
            <li>ASP.NET Core MVC</li>
            <li>Entity Framework</li>
            <li>LINQ</li>
        </ul>
        <li>資料庫：</li>
        <ul>
            <li>Microsoft SQL Server</li>
        </ul>
        <li>其他：</li>
        <ul>
            <li>Google Maps Embed API (嵌入地圖資訊)</li>
            <li>Google OAuth (帳號註冊登入)</li>
            <li>SignalR (即時通訊)</li>
            <li>Swiperjs (圖片切換互動)</li>
            <li>Animate.css (動畫效果)</li>
        </ul>
    </ul>
    <h2 id="database">資料庫設計：</h2>
    <img src="https://imgur.com/Q9z4vSy.png" alt="資料表關聯圖" style="max-width: 100%; height: auto;">
    <h2 id="feature">專案功能說明：</h2>
    <ul>
        <li>房間搜尋：透過篩選器快速找到符合自身需求的房間</li>
        <li>會員註冊：能使用一般郵件註冊，也能使用Google登入</li>
        <li>房間收藏：將心儀的房間加入收藏清單，節省過濾時間</li>
        <li>留言板：透過即時更新的留言板，了解彼此的契合度</li>
    </ul>
</body>

</html>
