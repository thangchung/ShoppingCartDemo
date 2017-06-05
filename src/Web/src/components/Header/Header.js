import React, { Component } from "react";
import { connect } from "react-redux";
import { Link } from "react-router";
import { Button } from "reactstrap";
import userManager from "../../utils/userManager";

class Header extends Component {
  sidebarToggle(e) {
    e.preventDefault();
    document.body.classList.toggle("sidebar-hidden");
  }

  sidebarMinimize(e) {
    e.preventDefault();
    document.body.classList.toggle("sidebar-minimized");
  }

  mobileSidebarToggle(e) {
    e.preventDefault();
    document.body.classList.toggle("sidebar-mobile-show");
  }

  asideToggle(e) {
    e.preventDefault();
    document.body.classList.toggle("aside-menu-hidden");
  }

  onLogoutButtonClicked = e => {
    e.preventDefault();
    userManager.removeUser(); // removes the user data from sessionStorage
  };

  render() {
    var { isAuth } = this.props;
    return (
      <header className="app-header navbar">
        {isAuth &&
          <button
            className="navbar-toggler mobile-sidebar-toggler d-lg-none"
            onClick={this.mobileSidebarToggle}
            type="button"
          >
            ☰
          </button>}

        <Link className="navbar-brand" to="/"><b>Shopping Cart Demo</b></Link>
        
        {!isAuth &&
          <ul className="nav navbar-nav d-md-down-none mr-auto">
            <li className="nav-item">
              <a
                className="nav-link navbar-toggler sidebar-toggler"
                onClick={this.sidebarToggle}
                href="#"
              >
                ☰
              </a>
            </li>
            <li className="nav-item px-1">
              <Link className="nav-link navbar-toggler sidebar-toggler" to="/payment">Payment</Link>
            </li>
            <li className="nav-item px-1">
              <Link className="nav-link navbar-toggler sidebar-toggler" to="/audit">Audit</Link>
            </li>
          </ul>}
          {!isAuth &&
          <Link className="pull-right" to="/login">Login</Link>}
        {isAuth &&
          <Button color="default" onClick={this.onLogoutButtonClicked}>
            Logout
          </Button>}
      </header>
    );
  }
}

function isAuth(state) {
  if (state.oidc.user) {
    return state.oidc.user != null;
  }
  return false;
}

function mapStateToProps(state, ownProps) {
  return {
    isAuth: isAuth(state)
  };
}

export default connect(mapStateToProps, null)(Header);
