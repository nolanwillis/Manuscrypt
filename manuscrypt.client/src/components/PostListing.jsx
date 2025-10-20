import React from "react";
import { Link } from "react-router-dom";

export function GetPostedTime(publishedAt) {
    const now = new Date();
    const published = new Date(publishedAt);
    const diffMs = now - published;
    const diffMinutes = Math.floor(diffMs / (1000 * 60));
    const diffHours = Math.floor(diffMs / (1000 * 60 * 60));
    const diffDays = Math.floor(diffMs / (1000 * 60 * 60 * 24));
    const diffWeeks = Math.floor(diffDays / 7);
    const diffMonths = Math.floor(diffDays / 30.44); // average month length
    const diffYears = Math.floor(diffDays / 365.25);

    if (diffMinutes < 60) {
        return `${diffMinutes <= 0 ? 1 : diffMinutes} minute${
            diffMinutes === 1 ? "" : "s"
        } ago`;
    } else if (diffHours < 24) {
        return `${diffHours} hour${diffHours === 1 ? "" : "s"} ago`;
    } else if (diffDays < 30) {
        return `${diffWeeks <= 0 ? 1 : diffWeeks} week${
            diffWeeks === 1 ? "" : "s"
        } ago`;
    } else if (diffDays < 365) {
        return `${diffMonths <= 0 ? 1 : diffMonths} month${
            diffMonths === 1 ? "" : "s"
        } ago`;
    } else {
        return `${diffYears} year${diffYears === 1 ? "" : "s"} ago`;
    }
}

export default function PostListing({ post }) {
    const postedTime = GetPostedTime(post.publishedAt);

    return (
        <Link to={`/post/${post.id}`} className="block">
            <div className="bg-white rounded-xl shadow flex flex-col items-stretch mb-6 border border-gray-200 w-60 h-300px">
                <div className="w-full h-48 bg-black rounded-t-xl flex items-center justify-center">
                    {/* Placeholder for future thumbnail or document preview */}
                </div>
                <div className="flex flex-col px-4 py-4">
                    <h3 className="text-2xl font-bold text-black mb-2">
                        {post.title}
                    </h3>
                    <div className="text-base font-semibold text-gray-800 mb-2">
                        {post.channelName}
                    </div>
                    <div className="flex flex-row items-center gap-2 text-base font-semibold text-gray-800">
                        <span>{post.views} views</span>
                        <span className="mx-1">&bull;</span>
                        <span>{postedTime}</span>
                    </div>
                </div>
            </div>
        </Link>
    );
}