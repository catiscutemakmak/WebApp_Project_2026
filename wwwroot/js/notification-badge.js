// ดึง unread count และแสดง badge
function updateNotificationBadge() {
    fetch('/notice/unread-count')
        .then(response => response.json())
        .then(data => {
            const badge = document.getElementById('notificationBadge');
            if (badge) {
                if (data.count > 0) {
                    badge.textContent = data.count;
                    badge.style.display = 'flex';
                } else {
                    badge.style.display = 'none';
                }
            }
        })
        .catch(error => console.log('Error updating badge:', error));
}

// เรียก function เมื่อ page โหลด
document.addEventListener('DOMContentLoaded', updateNotificationBadge);

// อัปเดต badge ทุก 5 วินาที
setInterval(updateNotificationBadge, 5000);