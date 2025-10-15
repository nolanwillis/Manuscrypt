import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import Layout from './components/Layout.jsx';
import Home from './components/Home.jsx';
import CreatePost from './components/CreatePost.jsx';
import Login from './components/Login.jsx';
import CreateAccount from './components/CreateAccount.jsx';
export default function App() {
    const [userId, setUserId] = useState(0);

    useEffect(() => {
        const storedUserId = sessionStorage.getItem('userId');
        setUserId(storedUserId);
    }, []);

    const handleLogoutClicked = () => {
        sessionStorage.removeItem('userId');
        setUserId(0);
    };

    const handleUserIdReceived = (userId) => {
        sessionStorage.setItem('userId', userId);
        setUserId(userId);
    };

    return (
        <Router>
            <Routes>
                <Route
                    path="/"
                    element={
                        <Layout userId={userId} logoutBtnHandler={handleLogoutClicked} />
                    }
                >
                    <Route index element={<Home />} />
                    <Route path="create-post" element={<CreatePost />} />
                    <Route path="login" element={<Login onUserIdReceived={handleUserIdReceived} />} />
                    <Route path="create-account" element={<CreateAccount onUserIdReceived={handleUserIdReceived} />} />
                </Route>i
                <Route path="*" element={<Navigate to="/" />} />
            </Routes>
        </Router>
    );
}