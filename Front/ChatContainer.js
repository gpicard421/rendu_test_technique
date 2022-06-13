const e = React.createElement;

class ChatPanel extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      input: "",
    };
  }

  HandleChange = (input) => {
    this.setState({ input: input.target.value });
  };

  Onsubmit = (e) => {
    if (this.state.input !== "" && (e.key == "Enter" || e.type === "click")) {
      const date = new Date();
      this.props.OnSubmit({
        user: this.props.user,
        date:
          date.getHours() +
          ":" +
          (date.getMinutes() < 10
            ? "0" + date.getMinutes()
            : date.getMinutes()),
        text: this.state.input,
      });
      this.setState({ input: "" });
    } else return;
  };

  ChatList = (props) => {
    const listItems = props.values.map((value, index) =>
      e(
        "li",
        { key: index },
        e("div", { className: "chat-body" }, [
          e(
            "div",
            {
              className: "header col-sm-1 align-top",
              id: this.props.user === value.user ? "rigth" : "",
            },
            [
              e("strong", { className: "primary-font" }, value.user),
              e("small", { className: "pull-right text-muted" }, [
                e("span", { className: "glyphicon glyphicon-time" }),
                value.date,
              ]),
            ]
          ),
          e(
            "div",
            {},
            e(
              "p",
              { id: this.props.user === value.user ? "prigth" : "" },
              value.text
            )
          ),
        ])
      )
    );
    return e("ul", { className: "chat" }, listItems);
  };

  render() {
    return e(
      "div",
      { className: "col-md-8", id: "chat-panel" },
      e("div", { className: "panel panel-primary" }, [
        e(
          "div",
          { id: "panel-heading" },
          e(
            "span",
            { className: "glyphicon glyphicon-user" },
            "Chat " + this.props.user
          )
        ),
        e(
          "div",
          { className: "panel-body" },
          e(this.ChatList, { values: this.props.values })
        ),
        e(
          "div",
          { className: "panel-footer" },
          e("div", { className: "input-group" }, [
            e("input", {
              className: "form-control input-sm",
              id: "btn-input",
              placeholder: "Type your message here..",
              value: this.state.input,
              onChange: this.HandleChange,
              onKeyDown: this.Onsubmit,
              className: "form-control",
            }),
            e(
              "span",
              { className: "input-group-btn" },
              e(
                "button",
                {
                  onClick: this.Onsubmit,
                  className: "button",
                  id: "btn-chat",
                },
                e("span", {}, "send")
              )
            ),
          ])
        ),
      ])
    );
  }
}

class ChatContainer extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      values: [],
    };
  }

  OnSubmit = (value) => {
    const values = this.state.values.slice();
    values.push(value);
    this.setState({ values: values });
  };

  render() {
    return e("div", { className: "d-flex justify-content-center" }, [
      e(ChatPanel, {
        user: "A",
        values: this.state.values,
        OnSubmit: this.OnSubmit,
      }),
      e("div", { className: "col-sm-1" }),
      e(ChatPanel, {
        user: "B",
        values: this.state.values,
        OnSubmit: this.OnSubmit,
      }),
    ]);
  }
}

const domContainer = document.querySelector("#container");
ReactDOM.render(e(ChatContainer), domContainer);
