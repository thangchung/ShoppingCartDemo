import React, { Component } from "react";
import { bindActionCreators } from "redux";
import { connect } from "react-redux";
import { Link } from "react-router";
import {
  Button,
  Card,
  CardText,
  CardBlock,
  CardTitle,
  CardSubtitle,
  Container,
  Row,
  Col,
  Table,
  Input
} from "reactstrap";
import * as homeActions from "../../redux/modules/home";

class ShoppingCart extends Component {
  render() {
    const {
      products,
      handleEmptyCartClick,
      handleRemoveProductFromCart
    } = this.props;
    return (
      <div>
        <Row>
          <Col xs="9">
            <div>
              <Table>
                <thead>
                  <tr>
                    <td>Name</td>
                    <td>Model</td>
                    <td>Price</td>
                    <td>Quantity</td>
                    <td>&nbsp;</td>
                  </tr>
                </thead>
                <tbody>
                  {products.map(p => (
                    <tr key={p.product.id}>
                      <td>{p.product.name}</td>
                      <td>{p.product.model}</td>
                      <td>${p.product.price}</td>
                      <td>{p.quantity}</td>
                      <td>
                        <Button
                          onClick={() => handleRemoveProductFromCart(p.product)}
                          size="sm"
                          color="warning"
                        >
                          Remove
                        </Button>
                      </td>
                    </tr>
                  ))}
                  <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td><b>Total</b>:</td>
                    <td>$1234</td>
                  </tr>
                </tbody>
              </Table>
            </div>
          </Col>
          <Col xs="3">
            <Button color="danger" onClick={() => handleEmptyCartClick()}>
              Empty cart
            </Button>
            <Button color="info"><Link to="/payment">Checkout</Link></Button>
          </Col>
        </Row>
      </div>
    );
  }
}

class ThreeItems extends Component {
  constructor() {
    super();
    this.state = {
      valueChange: 1
    };
  }

  shouldComponentUpdate(nextProps, nextState) {
    return false;
  }

  onQuantityChange(value) {
    this.state.valueChange = value;
  }

  render() {
    const { products, handleAddProductClick } = this.props;

    return (
      <div>
        <Row>
          {products.map(product => (
            <Col key={product.id} xs="4">
              <Card>
                <CardBlock>
                  <CardTitle>{product.name} </CardTitle>
                  <CardSubtitle>{product.model}</CardSubtitle>
                  <CardText>Price: ${product.price}</CardText>
                  <Row>
                    <Col xs="6">
                      <Input
                        type="textbox"
                        defaultValue={this.state.valueChange}
                        onChange={this.onQuantityChange.bind(this)}
                      />
                    </Col>
                    <Col xs="6">
                      <Button
                        color="success"
                        onClick={() =>
                          handleAddProductClick(
                            product,
                            this.state.valueChange
                          )}
                      >
                        Add to cart
                      </Button>
                    </Col>
                  </Row>
                </CardBlock>
              </Card>
            </Col>
          ))}
        </Row>
      </div>
    );
  }
}

class Home extends Component {
  componentDidMount() {
    this.props.getProducts();
  }

  handleAddProductClick(product, quantity) {
    this.props.addProductToCart(product, quantity);
  }

  handleEmptyCartClick() {
    this.props.emptyCart();
  }

  handleRemoveProductFromCart(product) {
    this.props.removeProductFromCart(product);
  }

  render() {
    const { products, cart, loading } = this.props.homeStore;
    let listProducts = "";

    if (loading === true) {
      listProducts = "Loading...";
    } else {
      let threeItems = [], index = 1;
      while (products.length > 0) {
        threeItems = products.splice(0, 3);
        listProducts = [
          <ThreeItems
            key={index}
            products={threeItems}
            handleAddProductClick={this.handleAddProductClick.bind(this)}
          />,
          listProducts
        ];
        index++;
      }
    }

    return (
      <div>
        <div style={styles.home}>
        <Card >
          <CardBlock>
          <CardTitle>Cart</CardTitle>
          <Container>
            {
              <ShoppingCart
                products={cart}
                handleEmptyCartClick={this.handleEmptyCartClick.bind(this)}
                handleRemoveProductFromCart={this.handleRemoveProductFromCart.bind(
                  this
                )}
              />
            }
          </Container>
          </CardBlock>
        </Card>
        </div>
        <Container>
          {listProducts}
        </Container>
      </div>
    );
  }
}

const styles = {
  home: {
    "margin-top": "20px"
  }
}

function mapStateToProps(state, ownProps) {
  return {
    homeStore: state.homeStore
  };
}

// bindActionCreators({ ...productActions, ...cartActions }, dispatch)
export const mapDispatchToProps = dispatch =>
  bindActionCreators(homeActions, dispatch);

export default connect(mapStateToProps, mapDispatchToProps)(Home);
