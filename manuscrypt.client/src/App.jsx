import React, { useState, useEffect } from 'react';
import CreatePost from './components/CreatePost.jsx'
import CreateChannel from './components/CreateChannel.jsx'
import Login from './components/Login.jsx'
import Navigation from './components/Navigation.jsx'

export default function App() {
    const [userId, setUserId] = useState(null)
    const [isLoginOpen, setIsLoginOpen] = useState(false);

    useEffect(() => {
        const storedUserId = sessionStorage.getItem('userId');
        setUserId(storedUserId);
    }, []);

    const handleLoginPress = () => {
        setIsLoginOpen(true);
    }

    const handleLogoutPress = () => {
        sessionStorage.removeItem('userId');
        setUserId(null);
    }

    const handleUserFound = (userId) => {
        setUserId(userId);
        setIsLoginOpen(false)
    }

    return (
        <div className="min-h-screen relative">
            <Navigation isLoggedIn={userId} onLoginPress={handleLoginPress} onLogoutPress={handleLogoutPress} />
            <main className="flex-1 flex items-center justify-center pl-128">
                {isLoginOpen && <Login onUserFound={handleUserFound} />}
            </main>
        </div>
    );
}
