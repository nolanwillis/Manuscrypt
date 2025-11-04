import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import Layout from './components/Layout.jsx';
import Home from './components/Home.jsx';
import CreatePost from './components/CreatePost.jsx';
import Login from './components/Login.jsx';
import CreateAccount from './components/CreateAccount.jsx';
import MyChannel from './components/MyAccount.jsx';
import Post from './components/Post.jsx'

export default function App() {
    const [isLoggedIn, setIsLoggedIn] = useState(false);

    useEffect(() => {
        const token = localStorage.getItem('authToken');
        if (token)
        {
            setIsLoggedIn(true);
        }
    }, []);

    const handleLogoutClicked = () => {
        localStorage.removeItem('');
        setIsLoggedIn(false);
    };

    const handleTokenReceived = () => {
        setIsLoggedIn(true);
    };

    return (
        <Router>
            <Routes>
                <Route
                    path="/"
                    element={
                        <Layout loggedInStatus={isLoggedIn} logoutBtnHandler={handleLogoutClicked} />
                    }
                >
                    <Route index element={<Home />} />
                    <Route path="my-account" element={<MyChannel />} />
                    <Route path="create-post" element={<CreatePost />} />
                    <Route path="login" element={<Login onTokenReceived={handleTokenReceived} />} />
                    <Route path="create-account" element={<CreateAccount onTokenReceived={handleTokenReceived} />} />
                    <Route path="post/:postId" element={<Post />} />
                </Route>
                <Route path="*" element={<Navigate to="/" />} />
            </Routes>
        </Router>
    );
}