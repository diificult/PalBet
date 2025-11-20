import { createPortal } from "react-dom";

export default function Modal({ children, open, onClose, className = "" }) {
    if (!open) return null;

    return createPortal(
        <div className={`fixed inset-0 z-50 ${className}`} onClick={onClose}>
            <div className="fixed inset-0 bg-gray-900/50"></div>
            <div className="fixed inset-0 flex items-center justify-center z-50">
                <div onClick={(e) => e.stopPropagation()}>{children}</div>
            </div>
        </div>,
        document.getElementById("modal")
    );
}
