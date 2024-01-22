importScripts('https://www.gstatic.com/firebasejs/9.14.0/firebase-app-compat.js')
importScripts('https://www.gstatic.com/firebasejs/9.14.0/firebase-messaging-compat.js')
const firebaseConfig = {
    apiKey: "AIzaSyAHCCIMhmGNIyzEHDR8Nc579zA49mY1g5A",
    authDomain: "purchasingms-8b968.firebaseapp.com",
    projectId: "purchasingms-8b968",
    storageBucket: "purchasingms-8b968.appspot.com",
    messagingSenderId: "287105718827",
    appId: "1:287105718827:web:a9e02d05f82fe61cb0a86e",
    measurementId: "G-6Q93J1QTDW"
};
const app = firebase.initializeApp(firebaseConfig)
const messaging = firebase.messaging()

messaging.onBackgroundMessage(function (payload) {
    if (!payload.hasOwnProperty('notification')) {
        const notificationTitle = payload.data.title
        const notificationOptions = {
            body: payload.data.body,
            icon: payload.data.icon,
            image: payload.data.image
        }
        self.registration.showNotification(notificationTitle, notificationOptions);
        self.addEventListener('notificationclick', function (event) {
            const clickedNotification = event.notification
            clickedNotification.close();
            event.waitUntil(
                clients.openWindow(payload.data.click_action)
            )
        })
    }
})