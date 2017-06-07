import React, { Component } from "react";
import { connect } from "react-redux";
import { Link, browserHistory } from "react-router";
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
    browserHistory.push("/");
  };

  onLoginButtonClicked = e => {
    e.preventDefault();
    browserHistory.push("/login");
  };

  render() {
    var { isAuth } = this.props;
    return (
      <header className="app-header navbar">
        <Link className="navbar-brand" to="/"><b>Shopping Cart</b></Link>

        {isAuth &&
          <ul className="nav navbar-nav d-md-down-none mr-auto">
            <li className="nav-item px-1">
              <Link
                className="nav-link navbar-toggler sidebar-toggler"
                to="admin/order"
              >
                Order
              </Link>
            </li>
            <li className="nav-item px-1">
              <Link
                className="nav-link navbar-toggler sidebar-toggler"
                to="admin/payment"
              >
                Payment
              </Link>
            </li>
            <li className="nav-item px-1">
              <Link
                className="nav-link navbar-toggler sidebar-toggler"
                to="admin/audit"
              >
                Audit
              </Link>
            </li>

          </ul>}

        <ul />
        {!isAuth &&
          <Button color="default" onClick={this.onLoginButtonClicked}>
            Login
          </Button>}
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
