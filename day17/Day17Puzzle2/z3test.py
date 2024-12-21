import z3 as z3

opt = z3.Optimize()
s = z3.BitVec('s', 64)
a, b, c = s, 0, 0

for x in [2,4,1,1,7,5,0,3,1,4,4,0,5,5,3,0]:
    # BST A
    b = a % 8
    # BXL 1
    b = b ^ 1
    # CDV B
    c = a / (1 << b)
    # ADV 3
    a = a / (1 << 3)
    # BXL 4
    b = b ^ 4
    # BXC
    b = b ^ c

    opt.add((b % 8) == x)

opt.add(a == 0)
opt.minimize(s)

if str(opt.check()) == "sat":
    print(opt.model().eval(s))