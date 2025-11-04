import React from "react";
import Navigation from "./Navigation.jsx";
import { Outlet } from "react-router-dom";

export default function Layout({ loggedInStatus, logoutBtnHandler }) {
    return (
        <div className="min-h-screen relative flex">
            <Navigation isLoggedIn={loggedInStatus} onLogoutClicked={logoutBtnHandler} />
            <main className="flex-1 flex items-center justify-center pl-128">
                <Outlet />
            </main>
        </div>
    );
}